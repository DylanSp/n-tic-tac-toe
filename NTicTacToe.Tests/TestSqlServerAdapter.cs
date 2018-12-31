using Adapters;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTicTacToe.Tests.Utilities;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestSqlServerAdapter
    {
        // TODO - have Adapter, ConnectionString as properties?

        // TODO - split this out
        [TestMethod]
        public void SerializeThenDeserialize_ShouldRoundTrip()
        {
            var data = new TicTacToeData();
            var serializer = new Serializer();
            var roundTripped = serializer.DeserializeGameData(serializer.SerializeGameData(data));
            Assert.AreEqual(data, roundTripped);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SaveThenRead_ShouldRoundTrip()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new SqlServerAdapter(connectionString);
            var gameData = new TicTacToeData();

            // change gameData to make sure Read() isn't just constructing new TicTacToeData()
            gameData.CurrentPlayer = Player.O;

            // Act
            adapter.Save(gameData);
            var (readSuccessfully, readData) = adapter.Read(gameData.Id);

            // Assert
            Assert.IsTrue(readSuccessfully);
            Assert.AreEqual(gameData, readData);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingNewObject_ShouldInsertOneRecord ()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new SqlServerAdapter(connectionString);
            var newGame = new TicTacToeData();
            int numPreExistingGames = AdapterTestHelpers.CountSqlGames(connectionString);

            // Act
            adapter.Save(newGame);

            // Assert
            int numPostExistingGames = AdapterTestHelpers.CountSqlGames(connectionString);
            Assert.AreEqual(numPreExistingGames + 1, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingExistingObject_ShouldPreserveNumberOfRecords ()
        {
            // Arrange

            // create new game and insert it, so we have something to update
            var gameToUpdate = new TicTacToeData();
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new SqlServerAdapter(connectionString);

            adapter.Save(gameToUpdate);

            int numPreExistingGames = AdapterTestHelpers.CountSqlGames(connectionString);

            // Act
            gameToUpdate.CurrentPlayer = Player.O;
            adapter.Save(gameToUpdate);

            // Assert
            int numPostExistingGames = AdapterTestHelpers.CountSqlGames(connectionString);
            Assert.AreEqual(numPreExistingGames, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void DeletingGame_ShouldRemoveOneRecord ()
        {
            // Arrange

            // create new game and insert it, so we have something to delete
            var gameToUpdate = new TicTacToeData();
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new SqlServerAdapter(connectionString);

            adapter.Save(gameToUpdate);

            int numPreExistingGames = AdapterTestHelpers.CountSqlGames(connectionString);

            // Act
            adapter.Delete(gameToUpdate.Id);

            // Assert
            int numPostExistingGames = AdapterTestHelpers.CountSqlGames(connectionString);
            Assert.AreEqual(numPreExistingGames - 1, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void ReadingAllGames_ShouldReturnAllGames()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var adapter = new SqlServerAdapter(connectionString);

            var insertedGames = new List<TicTacToeData>()
            {
                new TicTacToeData(),
                new TicTacToeData()
            };
            foreach(var game in insertedGames)
            {
                adapter.Save(game);
            }

            // Act
            var allGames = adapter.ReadAll();

            // Assert
            foreach(var game in insertedGames)
            {
                Assert.IsTrue(allGames.Contains(game));
            }
        }
    }
}
