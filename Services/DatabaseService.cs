using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private const int DatabaseTimeoutSeconds = 5;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            _cachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teachers_cache.json");
        }

        private NpgsqlConnection CreateConnection() // NpgsqlConnection для OpenAsync
        {
            return new NpgsqlConnection(_connectionString);
        }

        // Возвращает null при ошибке соединения/запроса, иначе список учителей
        public async Task<List<Teacher>?> GetTeachersFromDbAsync()
        {
            try
            {
                using var conn = CreateConnection();
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(DatabaseTimeoutSeconds));

                await conn.OpenAsync(cts.Token);

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
            catch (OperationCanceledException)
            {
                Console.WriteLine("[DB Service] Database connection timed out.");
                return null;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"[DB Service] NpgsqlException: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB Service] Generic exception in DB access: {ex.Message}");
                return null;
            }
        }

        public async Task SaveTeachersToCacheAsync(List<Teacher> teachers)
        {
            try
            {
                var json = JsonConvert.SerializeObject(teachers, Newtonsoft.Json.Formatting.Indented);
                await File.WriteAllTextAsync(_cachePath, json);
                Console.WriteLine("[DB Service] Data saved to cache.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB Service] Error saving to cache: {ex.Message}");
            }
        }

        public async Task<List<Teacher>?> LoadTeachersFromCacheAsync()
        {
            if (!File.Exists(_cachePath))
            {
                Console.WriteLine("[DB Service] Cache file not found.");
                return null;
            }
            try
            {
                var json = await File.ReadAllTextAsync(_cachePath);
                var teachers = JsonConvert.DeserializeObject<List<Teacher>>(json);
                Console.WriteLine($"[DB Service] Data loaded from cache. Teachers: {(teachers?.Count ?? 0)}");
                return teachers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB Service] Error loading from cache: {ex.Message}");
                return null;
            }
        }
    }
}