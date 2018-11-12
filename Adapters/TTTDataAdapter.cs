using System;
using Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace Data
{
    public class TTTDataAdapter : IGenericDataAdapter<ITicTacToeData>
    {
        private readonly string ConnectionString; // = ConfigurationManager.ConnectionStrings["dev"].ToString();

        public TTTDataAdapter (string connectionString)
        {
            ConnectionString = connectionString;
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
                stateParam.Value = SerializeGameData(newData);
                command.Parameters.Add(stateParam);

                var idParam = new SqlParameter("@id", SqlDbType.UniqueIdentifier);
                idParam.Value = newData.Id;
                command.Parameters.Add(idParam);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // can throw exceptions from opening connection, running query, deserialization
        public ITicTacToeData Read(Guid id)
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
                return DeserializeGameData(returnData);
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

        public static string SerializeGameData(ITicTacToeData data)
        {
            var serializer = new XmlSerializer(typeof(TicTacToeData));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }

        public static ITicTacToeData DeserializeGameData(string data)
        {
            var deserializer = new XmlSerializer(typeof(TicTacToeData));
            using (var reader = new StringReader(data))
            {
                return deserializer.Deserialize(reader) as TicTacToeData;
            }
        }
    }
}
