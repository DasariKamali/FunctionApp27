using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FunctionApp27.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlDatabase");
        }

        private async Task<DataTable> ExecuteQueryAsync(string query)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            using var adapter = new SqlDataAdapter(command);
            var dataTable = new DataTable();
            await Task.Run(() => adapter.Fill(dataTable));
            return dataTable;
        }

        public async Task<DataTable> GetVariationsAsync()
        {
            string query = "SELECT * FROM Table1";  
            return await ExecuteQueryAsync(query);
        }

        public async Task<DataTable> GetCommentariesAsync()
        {
            string query = "SELECT CommentaryID, Comment FROM Table2";  
            return await ExecuteQueryAsync(query);
        }
    }
}
