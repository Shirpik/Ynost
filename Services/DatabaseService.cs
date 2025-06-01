using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using Ynost.Models;

namespace Ynost.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly string _cachePath;
        private bool _isRefreshing = false;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            _cachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teachers_cache.json");
        }

        private IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);

        public async Task<List<Teacher>> GetAllTeachersAsync(bool useCache = true)
        {
            // Пробуем загрузить из кеша, если разрешено и файл существует
            if (useCache && File.Exists(_cachePath))
            {
                try
                {
                    var cachedData = await LoadFromCache();
                    if (cachedData != null && cachedData.Count > 0)
                    {
                        // Запускаем фоновое обновление без ожидания
                        _ = RefreshCacheInBackground();
                        return cachedData;
                    }
                }
                catch
                {
                    // При ошибке чтения кеша просто продолжаем
                }
            }

            // Если кеш недоступен - загружаем из БД
            return await LoadFromDatabaseAndSaveCache();
        }

        private async Task<List<Teacher>> LoadFromDatabaseAndSaveCache()
        {
            var teachers = await LoadTeachersFromDatabase();
            await SaveToCache(teachers);
            return teachers;
        }

        private async Task<List<Teacher>> LoadTeachersFromDatabase()
        {
            using var conn = CreateConnection();
            const string sql = @"
                SELECT 
                    teacher_id AS Id, 
                    full_name AS FullName, 
                    is_lecturer AS IsLecturer 
                FROM teachers 
                ORDER BY full_name;
            ";

            var result = await conn.QueryAsync<Teacher>(sql);
            return result.AsList();
        }

        private async Task SaveToCache(List<Teacher> teachers)
        {
            try
            {
                var json = JsonConvert.SerializeObject(teachers, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented);
                await File.WriteAllTextAsync(_cachePath, json);
            }
            catch
            {
                // Игнорируем ошибки записи кеша
            }
        }

        private async Task<List<Teacher>> LoadFromCache()
        {
            try
            {
                var json = await File.ReadAllTextAsync(_cachePath);
                return JsonConvert.DeserializeObject<List<Teacher>>(json);
            }
            catch
            {
                return null;
            }
        }

        private async Task RefreshCacheInBackground()
        {
            // Защита от множественного одновременного обновления
            if (_isRefreshing) return;
            _isRefreshing = true;

            try
            {
                await Task.Delay(1000); // Даем время на отрисовку UI
                await LoadFromDatabaseAndSaveCache();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        // Метод для принудительного обновления (если понадобится)
        public async Task ForceRefreshCache()
        {
            await LoadFromDatabaseAndSaveCache();
        }
    }
}