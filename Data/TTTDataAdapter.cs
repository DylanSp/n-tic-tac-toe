using Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace Data
{
    public class TTTDataAdapter : IDataAdapter<TicTacToeData>
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=TicTacToe;Persist Security Info=True;User ID=sa;Password=D0cupPhase1!";

        public int Create()
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public TicTacToeData Read(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = "SELECT gameState FROM GameData WHERE id = @id";
                var command = new SqlCommand(queryString, connection);

                var idParam = new SqlParameter("@id", SqlDbType.Int);
                idParam.Value = id;
                command.Parameters.Add(idParam);

                // let exceptions propagate up
                connection.Open();
                var returnData = (byte[])command.ExecuteScalar();
                return DeserializeGameData(returnData);
            }
        }

        public void Update(TicTacToeData newData)
        {
            throw new System.NotImplementedException();
        }

        public static byte[] SerializeGameData(TicTacToeData data)
        {
            var serializer = new XmlSerializer(typeof(TicTacToeData));
            using (var memStream = new MemoryStream())
            {
                serializer.Serialize(memStream, data);
                return memStream.ToArray();
            }
        }

        public static TicTacToeData DeserializeGameData(byte[] data)
        {
            var deserializer = new XmlSerializer(typeof(TicTacToeData));
            using (var memStream = new MemoryStream())
            {
                memStream.Write(data, 0, data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return (TicTacToeData)deserializer.Deserialize(memStream);
            }
        }
    }
}
