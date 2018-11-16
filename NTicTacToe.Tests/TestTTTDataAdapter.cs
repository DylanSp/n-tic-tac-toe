﻿using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestTTTDataAdapter
    {
        [TestMethod]
        public void SerializeThenDeserialize_ShouldRoundTrip()
        {
            var data = new TicTacToeData();
            var roundTripped = TTTDataAdapter.DeserializeGameData(TTTDataAdapter.SerializeGameData(data));
            Assert.AreEqual(data, roundTripped);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SaveThenRead_ShouldRoundTrip()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new TTTDataAdapter(connectionString);
            var gameData = new TicTacToeData();

            // change gameData to make sure Read() isn't just constructing new TicTacToeData()
            gameData.CurrentPlayer = Player.O;

            // Act
            adapter.Save(gameData);
            var readData = adapter.Read(gameData.Id);

            // Assert
            Assert.AreEqual(gameData, readData);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingNewObject_ShouldInsertOneRecord ()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new TTTDataAdapter(connectionString);
            var newGame = new TicTacToeData();
            int numPreExistingGames;

            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                numPreExistingGames = (int)command.ExecuteScalar();
            }

            // Act
            adapter.Save(newGame);

            // Assert
            int numPostExistingGames;
            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                numPostExistingGames = (int)command.ExecuteScalar();
            }

            Assert.AreEqual(numPreExistingGames + 1, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingExistingObject_ShouldPreserveNumberOfRecords ()
        {
            // Arrange

            // create new game and insert it, so we have something to update
            var gameToUpdate = new TicTacToeData();
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new TTTDataAdapter(connectionString);

            adapter.Save(gameToUpdate);

            int numPreExistingGames;
            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                numPreExistingGames = (int)command.ExecuteScalar();
            }

            // Act
            gameToUpdate.CurrentPlayer = Player.O;
            adapter.Save(gameToUpdate);

            // Assert
            int numPostExistingGames;
            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                numPostExistingGames = (int)command.ExecuteScalar();
            }
            Assert.AreEqual(numPreExistingGames, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void DeletingGame_ShouldRemoveOneRecord ()
        {
            // Arrange

            // create new game and insert it, so we have something to delete
            var gameToUpdate = new TicTacToeData();
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new TTTDataAdapter(connectionString);

            adapter.Save(gameToUpdate);

            int numPreExistingGames;
            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                numPreExistingGames = (int)command.ExecuteScalar();
            }

            // Act
            adapter.Delete(gameToUpdate.Id);

            // Assert
            int numPostExistingGames;
            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT COUNT(*) FROM GameData";
                var command = new SqlCommand(queryString, connection);

                connection.Open();
                numPostExistingGames = (int)command.ExecuteScalar();
            }
            Assert.AreEqual(numPreExistingGames - 1, numPostExistingGames);
        }
    }
}
