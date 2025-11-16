// Data/Database/DatabaseConnection.cs
using MySql.Data.MySqlClient;

namespace SportClubApp.Data.Database
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        public bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch { return false; }
        }
    }
}
