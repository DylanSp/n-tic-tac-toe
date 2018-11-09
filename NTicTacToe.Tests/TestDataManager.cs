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
        private IGameManager GameManager;
        private IGenericDataAdapter<ITicTacToeData> Adapter;
        private ITicTacToeData TestData;
        private Guid TestId;

        [TestInitialize]
        public void BeforeEach()
        {
            TestData = new TicTacToeData();
            TestId = TestData.Id;
            GameManager = Substitute.For<IGameManager>();
            Adapter = Substitute.For<IGenericDataAdapter<ITicTacToeData>>();
            DataManager = new DataManager(GameManager, Adapter);
        }

        [TestMethod]
        public void GetGameData_ShouldReturnCorrectData()
        {
            // Arrange
            Adapter.Read(TestId).Returns(TestData);
            GameManager.GameData.Returns(TestData);

            // Act
            var readData = DataManager.GetGameData();

            // Assert
            Assert.AreEqual(TestId, readData.Id);
        }

        [TestMethod]
        public void CreatingGame_ShouldSetUpNewGame()
        {
            // Arrange
            GameManager.GameData.CurrentPlayer.Returns(Player.X);
            GameManager.GameData.Result.Returns(GameResult.Unfinished);
            GameManager.GameData.Board.Returns(Enumerable.Repeat(CellState.Empty, 9).ToList());

            // Act
            DataManager.CreateAndSaveGame();

            // Assert
            var gameData = DataManager.GameManager.GameData;
            Assert.AreEqual(Player.X, gameData.CurrentPlayer);
            Assert.AreEqual(GameResult.Unfinished, gameData.Result);
            var emptyBoard = Enumerable.Repeat(CellState.Empty, 9).ToList();
            Assert.IsTrue(emptyBoard.SequenceEqual(gameData.Board));
        }
    }
}
