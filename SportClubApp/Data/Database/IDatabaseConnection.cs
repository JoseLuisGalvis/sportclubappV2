// Data/Database/IDatabaseConnection.cs
using MySql.Data.MySqlClient;

namespace SportClubApp.Data.Database
{
    public interface IDatabaseConnection
    {
        MySqlConnection GetConnection();
        bool TestConnection();
    }
}
