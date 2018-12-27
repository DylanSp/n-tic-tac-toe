using Adapters;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTicTacToe.Tests.Utilities;
using StackExchange.Redis;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestSqlServerCachedAdapter
    {
        [TestMethod, TestCategory("IntegrationTest")]
        public void SaveThenRead_ShouldRoundTrip()
        {
            // Arrange
            var sqlConnectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var redisConnectionString = ConfigurationManager.ConnectionStrings["redis-test"].ToString();
            var sqlAdapter = new SqlServerAdapter(sqlConnectionString);
            var redisAdapter = new RedisAdapter(redisConnectionString);
            var cachedAdapter = new SqlServerCachedAdapter(sqlAdapter, redisAdapter);
            var gameData = new TicTacToeData();

            // change gameData to make sure Read() isn't just constructing new TicTacToeData()
            gameData.CurrentPlayer = Player.O;

            // Act
            cachedAdapter.Save(gameData);
            var (readSuccessfully, readData) = cachedAdapter.Read(gameData.Id);

            // Assert
            Assert.IsTrue(readSuccessfully);
            Assert.AreEqual(gameData, readData);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingNewObject_ShouldInsertOneRecordInPrimaryAndCache()
        {
            // Arrange
            var sqlConnectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var redisConnectionString = ConfigurationManager.ConnectionStrings["redis-test"].ToString();
            var sqlAdapter = new SqlServerAdapter(sqlConnectionString);
            var redisAdapter = new RedisAdapter(redisConnectionString);
            var cachedAdapter = new SqlServerCachedAdapter(sqlAdapter, redisAdapter);

            var newGame = new TicTacToeData();
            int numPreExistingGamesSql = AdapterTestHelpers.CountSqlGames(sqlConnectionString);
            int numPreExistingGamesRedis = AdapterTestHelpers.CountRedisGames(redisConnectionString);

            // Act
            cachedAdapter.Save(newGame);

            // Assert
            int numPostExistingGamesSql = AdapterTestHelpers.CountSqlGames(sqlConnectionString);
            int numPostExistingGamesRedis = AdapterTestHelpers.CountRedisGames(redisConnectionString);

            Assert.AreEqual(numPreExistingGamesSql + 1, numPostExistingGamesSql);
            Assert.AreEqual(numPreExistingGamesRedis + 1, numPostExistingGamesRedis);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void SavingExistingObject_ShouldPreserveNumberOfRecordsInPrimaryAndCache()
        {
            // Arrange
            var sqlConnectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var redisConnectionString = ConfigurationManager.ConnectionStrings["redis-test"].ToString();
            var sqlAdapter = new SqlServerAdapter(sqlConnectionString);
            var redisAdapter = new RedisAdapter(redisConnectionString);
            var cachedAdapter = new SqlServerCachedAdapter(sqlAdapter, redisAdapter);

            // create new game and insert it, so we have something to update
            var gameToUpdate = new TicTacToeData();

            cachedAdapter.Save(gameToUpdate);

            int numPreExistingGamesSql = AdapterTestHelpers.CountSqlGames(sqlConnectionString);
            int numPreExistingGamesRedis = AdapterTestHelpers.CountRedisGames(redisConnectionString);

            // Act
            gameToUpdate.CurrentPlayer = Player.O;
            cachedAdapter.Save(gameToUpdate);

            // Assert
            int numPostExistingGamesSql = AdapterTestHelpers.CountSqlGames(sqlConnectionString);
            int numPostExistingGamesRedis = AdapterTestHelpers.CountRedisGames(redisConnectionString);
            Assert.AreEqual(numPreExistingGamesSql, numPostExistingGamesSql);
            Assert.AreEqual(numPreExistingGamesRedis, numPostExistingGamesRedis);
        }

        [TestMethod, TestCategory("IntegrationTest")]
        public void DeletingGame_ShouldRemoveOneRecordInPrimaryAndCache()
        {
            // Arrange
            var sqlConnectionString = ConfigurationManager.ConnectionStrings["test"].ToString();
            var redisConnectionString = ConfigurationManager.ConnectionStrings["redis-test"].ToString();
            var sqlAdapter = new SqlServerAdapter(sqlConnectionString);
            var redisAdapter = new RedisAdapter(redisConnectionString);
            var cachedAdapter = new SqlServerCachedAdapter(sqlAdapter, redisAdapter);

            // create new game and insert it, so we have something to delete
            var gameToUpdate = new TicTacToeData();

            cachedAdapter.Save(gameToUpdate);

            int numPreExistingGamesSql = AdapterTestHelpers.CountSqlGames(sqlConnectionString);
            int numPreExistingGamesRedis = AdapterTestHelpers.CountRedisGames(redisConnectionString);

            // Act
            cachedAdapter.Delete(gameToUpdate.Id);

            // Assert
            int numPostExistingGamesSql = AdapterTestHelpers.CountSqlGames(sqlConnectionString);
            int numPostExistingGamesRedis = AdapterTestHelpers.CountRedisGames(redisConnectionString);
            Assert.AreEqual(numPreExistingGamesSql - 1, numPostExistingGamesSql);
            Assert.AreEqual(numPreExistingGamesRedis - 1, numPostExistingGamesRedis);
        }
    }
}
