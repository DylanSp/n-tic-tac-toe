using Adapters;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTicTacToe.Tests.Utilities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestRedisAdapter
    {
        private RedisAdapter Adapter;
        private string ConnectionString;

        [TestInitialize]
        public void Setup()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["redis-test"].ToString();
            Adapter = new RedisAdapter(ConnectionMultiplexer.Connect(ConnectionString));
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SaveThenRead_ShouldRoundTrip()
        {
            // Arrange
            var gameData = new TicTacToeData();

            // change gameData to make sure Read() isn't just constructing new TicTacToeData()
            gameData.CurrentPlayer = Player.O;

            // Act
            Adapter.Save(gameData);
            var (readSuccessfully, readData) = Adapter.Read(gameData.Id);

            // Assert
            Assert.IsTrue(readSuccessfully);
            Assert.AreEqual(gameData, readData);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingNewObject_ShouldInsertOneRecord()
        {
            // Arrange
            int numPreExistingGames = AdapterTestHelpers.CountRedisGames(ConnectionString);

            // Act
            var newGame = new TicTacToeData();
            Adapter.Save(newGame);

            // Assert
            int numPostExistingGames = AdapterTestHelpers.CountRedisGames(ConnectionString);
            Assert.AreEqual(numPreExistingGames + 1, numPostExistingGames);
        }

        
        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingExistingObject_ShouldPreserveNumberOfRecords()
        {
            // Arrange

            // create new game and insert it, so we have something to update
            var gameToUpdate = new TicTacToeData();
            Adapter.Save(gameToUpdate);
            int numPreExistingGames = AdapterTestHelpers.CountRedisGames(ConnectionString);

            // Act
            gameToUpdate.CurrentPlayer = Player.O;
            Adapter.Save(gameToUpdate);

            // Assert
            int numPostExistingGames = AdapterTestHelpers.CountRedisGames(ConnectionString);
            Assert.AreEqual(numPreExistingGames, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void DeletingGame_ShouldRemoveOneRecord()
        {
            // Arrange

            // create new game and insert it, so we have something to delete
            var gameToUpdate = new TicTacToeData();
            Adapter.Save(gameToUpdate);
            int numPreExistingGames = AdapterTestHelpers.CountRedisGames(ConnectionString);

            // Act
            Adapter.Delete(gameToUpdate.Id);

            // Assert
            int numPostExistingGames = AdapterTestHelpers.CountRedisGames(ConnectionString);
            Assert.AreEqual(numPreExistingGames - 1, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void ReadingAllGames_ShouldReturnAllGames()
        {
            // Arrange
            var insertedGames = new List<TicTacToeData>()
            {
                new TicTacToeData(),
                new TicTacToeData()
            };
            foreach (var game in insertedGames)
            {
                Adapter.Save(game);
            }

            // Act
            var allGames = Adapter.ReadAll();

            // Assert
            foreach (var game in insertedGames)
            {
                Assert.IsTrue(allGames.Contains(game));
            }
        }
    }
}
