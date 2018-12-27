using StackExchange.Redis;
using System.Data.SqlClient;
using System.Linq;

namespace NTicTacToe.Tests.Utilities
{
    public static class AdapterTestHelpers
    {
        public static int CountSqlGames(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        } 

        public static int CountRedisGames(string connectionString)
        {
            var conn = ConnectionMultiplexer.Connect(connectionString);
            var endpoints = conn.GetEndPoints();
            var server = conn.GetServer(endpoints[0]);
            return server.Keys().Count();
        }
    }
}
