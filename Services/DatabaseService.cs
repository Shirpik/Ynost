using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Ynost.Models;

namespace Ynost.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            // Обычный using – соединение будет корректно закрыто и очищено
            using var conn = CreateConnection();
            // Не обязательно делать conn.OpenAsync(), Dapper сам откроет его если нужно

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
    }
}
