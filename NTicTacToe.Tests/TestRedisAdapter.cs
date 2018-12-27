using Adapters;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Adapter = new RedisAdapter(ConnectionString);
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
            var readData = Adapter.Read(gameData.Id);

            // Assert
            Assert.AreEqual(gameData, readData);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingNewObject_ShouldInsertOneRecord()
        {
            // Arrange
            var conn = ConnectionMultiplexer.Connect(ConnectionString);
            var endpoints = conn.GetEndPoints();
            var server = conn.GetServer(endpoints[0]);
            int numPreExistingGames = server.Keys().Count();

            // Act
            var newGame = new TicTacToeData();
            Adapter.Save(newGame);

            // Assert
            int numPostExistingGames = server.Keys().Count();
            Assert.AreEqual(numPreExistingGames + 1, numPostExistingGames);
        }

        
        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingExistingObject_ShouldPreserveNumberOfRecords()
        {
            // Arrange
            var conn = ConnectionMultiplexer.Connect(ConnectionString);
            var endpoints = conn.GetEndPoints();
            var server = conn.GetServer(endpoints[0]);

            // create new game and insert it, so we have something to update
            var gameToUpdate = new TicTacToeData();
            Adapter.Save(gameToUpdate);
            int numPreExistingGames = server.Keys().Count();

            // Act
            gameToUpdate.CurrentPlayer = Player.O;
            Adapter.Save(gameToUpdate);

            // Assert
            int numPostExistingGames = server.Keys().Count();
            Assert.AreEqual(numPreExistingGames, numPostExistingGames);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void DeletingGame_ShouldRemoveOneRecord()
        {
            // Arrange
            var conn = ConnectionMultiplexer.Connect(ConnectionString);
            var endpoints = conn.GetEndPoints();
            var server = conn.GetServer(endpoints[0]);

            // create new game and insert it, so we have something to delete
            var gameToUpdate = new TicTacToeData();
            Adapter.Save(gameToUpdate);
            int numPreExistingGames = server.Keys().Count();

            // Act
            Adapter.Delete(gameToUpdate.Id);

            // Assert
            int numPostExistingGames = server.Keys().Count();
            Assert.AreEqual(numPreExistingGames - 1, numPostExistingGames);
        }
    }
}
