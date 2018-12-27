using System;
using Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using Data;

namespace Adapters
{
    public class SqlServerAdapter : IGenericDataAdapter<ITicTacToeData>
    {
        private readonly Serializer serializer;
        private readonly string ConnectionString;

        // future TODO - instead of taking string,
        // take some sort of context object that validates connection string
        public SqlServerAdapter (string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            ConnectionString = connectionString;

            serializer = new Serializer();
        }

        // can throw exceptions from opening connection, running query, deserialization
        public void Save(ITicTacToeData newData)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"IF EXISTS ( SELECT * FROM GameData WHERE Id = @id)
    UPDATE GameData SET GameState = @state WHERE Id = @id
ELSE
    INSERT GameData (Id, GameState) VALUES (@id, @state)";
                var command = new SqlCommand(queryString, connection);

                var stateParam = new SqlParameter("@state", SqlDbType.NVarChar);
                stateParam.Value = serializer.SerializeGameData(newData);
                command.Parameters.Add(stateParam);

                var idParam = new SqlParameter("@id", SqlDbType.UniqueIdentifier);
                idParam.Value = newData.Id;
                command.Parameters.Add(idParam);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // can throw exceptions from opening connection, running query, deserialization
        public (bool, ITicTacToeData) Read(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = "SELECT GameState FROM GameData WHERE Id = @id";
                var command = new SqlCommand(queryString, connection);

                var idParam = new SqlParameter("@id", SqlDbType.UniqueIdentifier);
                idParam.Value = id;
                command.Parameters.Add(idParam);

                connection.Open();
                var returnData = (string)command.ExecuteScalar();
                if (returnData == null)
                {
                    return (false, new EmptyTicTacToeData());
                }
                else
                {
                    return (true, serializer.DeserializeGameData(returnData));
                }
            }
        }

        // can throw exceptions from opening connection, running query, deserialization
        public void Delete(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = "DELETE FROM GameData WHERE Id = @id";
                var command = new SqlCommand(queryString, connection);

                var idParam = new SqlParameter("@id", SqlDbType.UniqueIdentifier);
                idParam.Value = id;
                command.Parameters.Add(idParam);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
