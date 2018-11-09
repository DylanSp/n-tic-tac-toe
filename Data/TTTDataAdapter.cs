﻿using System;
using Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Data
{
    public class TTTDataAdapter : IGenericDataAdapter<TicTacToeData>
    {
        // private const string ConnectionString = "Data Source=localhost;Initial Catalog=TicTacToe;Persist Security Info=True;User ID=sa;Password=D0cupPhase1!";
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=TicTacToe;Persist Security Info=True;User ID=sa;Password=adminpass";

        // can throw exceptions from opening connection, running query, deserialization
        public void Save(TicTacToeData newData)
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
        public TicTacToeData Read(Guid id)
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

        public static string SerializeGameData(TicTacToeData data)
        {
            var serializer = new XmlSerializer(typeof(TicTacToeData));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }

        public static TicTacToeData DeserializeGameData(string data)
        {
            var deserializer = new XmlSerializer(typeof(TicTacToeData));
            using (var reader = new StringReader(data))
            {
                return deserializer.Deserialize(reader) as TicTacToeData;
            }
        }
    }
}
