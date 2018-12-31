using Data;
using Interfaces;
using Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestDataManager
    {
        private DataManager DataManager;
        private IGameManagerFactory GameManagerFactory;
        private IGenericDataAdapter<ITicTacToeData> Adapter;
        private ITicTacToeData TestData;
        private Guid TestId;

        [TestInitialize]
        public void BeforeEach()
        {
            TestData = new TicTacToeData();
            TestId = TestData.Id;
            GameManagerFactory = Substitute.For<IGameManagerFactory>();
            Adapter = Substitute.For<IGenericDataAdapter<ITicTacToeData>>();
            DataManager = new DataManager(GameManagerFactory, Adapter);
        }

        [TestMethod]
        public void GetGameData_ShouldReturnCorrectData()
        {
            // Arrange
            Adapter.Read(TestId).Returns((true, TestData));
            var gameManager = Substitute.For<IGameManager>();
            gameManager.GameData.Returns(TestData);
            GameManagerFactory.CreateGameManager(TestData).Returns(gameManager);

            // Act
            var readData = DataManager.GetGameData(TestId);

            // Assert
            Assert.AreEqual(TestId, readData.Id);
        }

        [TestMethod]
        public void CreatingGame_ShouldSetUpNewGame()
        {
            // Arrange
            var gameManager = Substitute.For<IGameManager>();
            GameManagerFactory.CreateGameManager(Arg.Any<ITicTacToeData>()).Returns(gameManager);

            // Act
            DataManager.CreateAndSaveGame();

            // Assert
            gameManager.Received().ResetGame();
        }

        [TestMethod]
        public void GetAllGames_ShouldReturnAllGames()
        {
            // Arrange
            var gameList = new List<ITicTacToeData>()
            {
                new TicTacToeData(),
                new TicTacToeData()
            };
            Adapter.ReadAll().Returns(gameList);

            // Act
            var allGames = DataManager.GetAllGamesData();

            // Assert
            Assert.AreEqual(gameList.Count(), allGames.Count());
        }
    }
}
