using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Newtonsoft.Json;
using Ynost.Models;

namespace Ynost.Services
{
    public class DatabaseService
    {
        private readonly string _cs;
        private readonly string _cachePath;
        private const int TimeoutSec = 10;

        public DatabaseService(string connectionString)
        {
            _cs = connectionString;
            _cachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teachers_cache.json");

            // Включаем маппинг snake_case → PascalCase автоматически
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        #region helpers
        private static NpgsqlConnection Conn(string cs) => new(cs);

        private static string Today() => DateTime.Now.ToString("dd.MM.yyyy");
        #endregion

        #region LOAD
        public async Task<List<Teacher>?> LoadAllAsync()
        {
            try
            {
                using var db = Conn(_cs);
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutSec));
                await db.OpenAsync(cts.Token);

                // 1) Загружаем всех преподавателей-корневых:
                var teachers = (await db.QueryAsync<Teacher>(
                    @"SELECT id, full_name, is_lecturer 
              FROM teachers 
              ORDER BY full_name"))
                    .ToDictionary(t => t.Id);

                if (!teachers.Any())
                    return teachers.Values.ToList();

                // 2) Локальный метод, который «безопасно» загрузит дочерние строки
                async Task LoadChildAsync<T>(string table, Action<Teacher, IEnumerable<T>> add)
                    where T : class
                {
                    try
                    {
                        // забираем все записи
                        var rows = await db.QueryAsync<T>($"SELECT * FROM {table}");
                        foreach (var row in rows)
                        {
                            var propInfo = typeof(T).GetProperty("TeacherId");
                            if (propInfo == null)
                            {
                                Logger.Write($"[DB-LOAD] Модель {typeof(T).Name} не имеет свойства TeacherId => пропускаем.");
                                continue;
                            }

                            var rawValue = propInfo.GetValue(row);
                            if (rawValue == null)
                            {
                                Logger.Write($"[DB-LOAD] {table}: у строки TeacherId == null => пропускаем.");
                                continue;
                            }

                            var tid = (Guid)rawValue;
                            if (teachers.TryGetValue(tid, out var tch))
                            {
                                add(tch, new[] { row });
                            }
                            else
                            {
                                Logger.Write($"[DB-LOAD] {table}: TeacherId {tid} не найден среди загруженных преподавателей => пропускаем.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(ex, $"DB-LOAD {table}");
                    }
                }

                // 2. Загрузим все 14 дочерних таблиц
                await LoadChildAsync<AcademicYearResult>("academic_results",
                     (t, r) => t.AcademicResults.AddRange(r));

                await LoadChildAsync<GiaResult>("gia_results",
                     (t, r) => t.GiaResults.AddRange(r));

                await LoadChildAsync<DemoExamResult>("demo_exam_results",
                     (t, r) => t.DemoExamResults.AddRange(r));

                await LoadChildAsync<IndependentAssessment>("independent_assessments",
                     (t, r) => t.IndependentAssessments.AddRange(r));

                await LoadChildAsync<SelfDeterminationActivity>("self_determinations",
                     (t, r) => t.SelfDeterminations.AddRange(r));

                await LoadChildAsync<StudentOlympiad>("student_olympiads",
                     (t, r) => t.StudentOlympiads.AddRange(r));

                await LoadChildAsync<JuryActivity>("jury_activities",
                     (t, r) => t.JuryActivities.AddRange(r));

                await LoadChildAsync<MasterClass>("master_classes",
                     (t, r) => t.MasterClasses.AddRange(r));

                await LoadChildAsync<Speech>("speeches",
                     (t, r) => t.Speeches.AddRange(r));

                await LoadChildAsync<Publication>("publications",
                     (t, r) => t.Publications.AddRange(r));

                await LoadChildAsync<ExperimentalProject>("experimental_projects",
                     (t, r) => t.ExperimentalProjects.AddRange(r));

                await LoadChildAsync<Mentorship>("mentorships",
                     (t, r) => t.Mentorships.AddRange(r));

                await LoadChildAsync<ProgramMethodSupport>("program_supports",
                     (t, r) => t.ProgramSupports.AddRange(r));

                await LoadChildAsync<ProfessionalCompetition>("professional_competitions",
                     (t, r) => t.ProfessionalCompetitions.AddRange(r));

                return teachers.Values.ToList();
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "DB-LOAD");
                return null;
            }
        }
        #endregion

        #region SAVE
        public async Task<bool> SaveAllAsync(IEnumerable<Teacher> teachers)
        {
            try
            {
                await using var db = Conn(_cs);
                await db.OpenAsync();

                await using var tx = await db.BeginTransactionAsync();

                foreach (var t in teachers)
                {
                    try
                    {
                        Logger.Write($"[DB-SAVE] Сохраняем преподавателя: {t.FullName}, дочерних записей: " +
                                     $"{t.AcademicResults.Count} Acad, {t.GiaResults.Count} GIA, " +
                                     $"{t.DemoExamResults.Count} DemoExam, ...");

                        // 1) Upsert Teacher
                        const string upsertTeacher = @"
    INSERT INTO teachers (id, full_name, is_lecturer)
    VALUES (@Id, @FullName, @IsLecturer)
    ON CONFLICT (id) DO UPDATE SET
      full_name   = EXCLUDED.full_name,
      is_lecturer = EXCLUDED.is_lecturer;
                        ";
                        await db.ExecuteAsync(upsertTeacher, t, tx);

                        // 2) Сразу же «удаляем + вставляем» для каждой дочерней таблицы
                        await ReplaceAsync(db, tx, "academic_results", t.Id, t.AcademicResults);
                        await ReplaceAsync(db, tx, "gia_results", t.Id, t.GiaResults);
                        await ReplaceAsync(db, tx, "demo_exam_results", t.Id, t.DemoExamResults);
                        await ReplaceAsync(db, tx, "independent_assessments", t.Id, t.IndependentAssessments);
                        await ReplaceAsync(db, tx, "self_determinations", t.Id, t.SelfDeterminations);
                        await ReplaceAsync(db, tx, "student_olympiads", t.Id, t.StudentOlympiads);
                        await ReplaceAsync(db, tx, "jury_activities", t.Id, t.JuryActivities);
                        await ReplaceAsync(db, tx, "master_classes", t.Id, t.MasterClasses);
                        await ReplaceAsync(db, tx, "speeches", t.Id, t.Speeches);
                        await ReplaceAsync(db, tx, "publications", t.Id, t.Publications);
                        await ReplaceAsync(db, tx, "experimental_projects", t.Id, t.ExperimentalProjects);
                        await ReplaceAsync(db, tx, "mentorships", t.Id, t.Mentorships);
                        await ReplaceAsync(db, tx, "program_supports", t.Id, t.ProgramSupports);
                        await ReplaceAsync(db, tx, "professional_competitions", t.Id, t.ProfessionalCompetitions);
                    }
                    catch (Exception exInner)
                    {
                        Logger.Write(exInner, $"DB-SAVE TeacherId={t.Id}");
                        // Если ошибка на одном преподавателе — продолжаем с другими,
                        // но можно также прервать, если нужна жёсткая целостность:
                        // throw;
                    }
                }

                await tx.CommitAsync();
                Logger.Write("[DB] SaveAllAsync: all commits successful");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "DB-SAVE");
                return false;
            }
        }

        /// <summary>
        /// Универсальный «удалить-и-вставить» для дочерней таблицы table,
        /// куда передаётся teacherId (чтобы удалить все строки по нему) и новый список rows.
        /// Здесь мы оборачиваем каждую колонку в двойные кавычки, чтобы, например, "group" не вызывало ошибок.
        /// </summary>
        private static async Task ReplaceAsync<T>(
      IDbConnection db,
      IDbTransaction tx,
      string table,
      Guid teacherId,
      IEnumerable<T> rows)
        {
            // Сразу создаём простую List<T>, чтобы избежать проблем с изменением коллекции во время ExecuteAsync
            var rowList = rows.ToList();

            try
            {
                // 1. Удаляем старые строки
                string deleteSql = $"DELETE FROM {table} WHERE teacher_id = @teacherId";
                Logger.Write($"[DB-REPLACE] {table}: executing DELETE");
                await db.ExecuteAsync(deleteSql, new { teacherId }, tx);

                // 2. Если новых строк нет — выходим
                if (!rowList.Any())
                {
                    Logger.Write($"[DB-REPLACE] {table}: нет строк для вставки, выходим");
                    return;
                }

                // 3. Собираем колонки и параметры
                var props = typeof(T).GetProperties();
                var columnNames = props
                    .Select(p =>
                    {
                        var snake = p.Name.ToSnake();
                        return snake == "group" ? $"\"{snake}\"" : snake;
                    })
                    .ToList();
                string columns = string.Join(",", columnNames);
                string values = string.Join(",", props.Select(p => "@" + p.Name));

                var insertSql = $"INSERT INTO {table} ({columns}) VALUES ({values});";
                Logger.Write($"[DB-REPLACE] {table}: executing INSERT, rows count: {rowList.Count}");
                Logger.Write($"[DB-REPLACE] {table}: SQL = {insertSql}");

                await db.ExecuteAsync(insertSql, rowList, tx);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, $"DB-REPLACE {table} TeacherId={teacherId}");
                // Если хотите, можете снова пробросить («throw;»), чтобы SaveAllAsync сразу упал и вернул false
            }
        }

        #endregion

        #region CACHE
        public async Task SaveToCacheAsync(List<Teacher> teachers)
        {
            try
            {
                var json = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                await File.WriteAllTextAsync(_cachePath, json);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "CACHE-SAVE");
            }
        }

        public async Task<List<Teacher>?> LoadFromCacheAsync()
        {
            if (!File.Exists(_cachePath))
                return null;
            try
            {
                var json = await File.ReadAllTextAsync(_cachePath);
                return JsonConvert.DeserializeObject<List<Teacher>>(json);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "CACHE-LOAD");
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Extension для преобразования PascalCase → snake_case
    /// </summary>
    internal static class SnakeHelper
    {
        public static string ToSnake(this string s) =>
            string.Concat(s.Select((c, i) =>
                i > 0 && char.IsUpper(c)
                    ? "_" + char.ToLowerInvariant(c)
                    : char.ToLowerInvariant(c).ToString()));
    }
}
