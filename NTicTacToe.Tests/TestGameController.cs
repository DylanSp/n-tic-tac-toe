using Interfaces;
using Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NTicTacToe.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http.Results;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestGameController
    {
        [TestMethod]
        public void GetAllGames_ShouldReturnAllGameIds()
        {
            // Arrange
            var mockManager = Substitute.For<IDataManager>();
            var mockGame1 = Substitute.For<ITicTacToeData>();
            var mockId1 = Guid.NewGuid();
            mockGame1.Id.Returns(mockId1);
            var mockGame2 = Substitute.For<ITicTacToeData>();
            var mockId2 = Guid.NewGuid();
            mockGame2.Id.Returns(mockId2);
            var mockGameData = new List<ITicTacToeData>()
            {
                mockGame1,
                mockGame2
            };
            mockManager.GetAllGamesData().Returns(mockGameData);

            var controller = new GameController(mockManager);

            // Act
            var result = controller.GetAllGames() as OkNegotiatedContentResult<JArray>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(mockGameData.Count, result.Content.Count());

            for (var i = 0; i < mockGameData.Count(); i++)
            {
                Assert.IsTrue(result.Content[i].ToString() == mockGameData[i].Id.ToString());
            }
        }

        // TODO - get all games should return 500 on DB error

        [TestMethod]
        public void CreateGame_ShouldSaveGame()
        {
            // Arrange
            var mockAdapter = Substitute.For<IGenericDataAdapter<ITicTacToeData>>();
            var mockGameManagerFactory = Substitute.For<IGameManagerFactory>();
            var dataManager = new DataManager(mockGameManagerFactory, mockAdapter);
            var controller = new GameController(dataManager);

            // Act
            var result = controller.CreateGame() as OkNegotiatedContentResult<JObject>;

            // Assert
            mockAdapter.Received().Save(Arg.Any<ITicTacToeData>());
        }

        [TestMethod]
        public void CreateGame_ShouldReturnGuid()
        {
            // Arrange
            var mockManager = Substitute.For<IDataManager>();
            var guid = Guid.NewGuid();
            mockManager.CreateAndSaveGame().Returns(guid);
            var controller = new GameController(mockManager);

            // Act
            var result = controller.CreateGame() as OkNegotiatedContentResult<JObject>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(guid.ToString(), result.Content["Id"].ToString());
        }

        [TestMethod]
        public void CreateGame_ShouldReturnServerErrorOnDBError()
        {
            // Arrange
            var mockManager = Substitute.For<IDataManager>();
            mockManager.CreateAndSaveGame().Returns(x => { throw new MockDbException(); });
            var controller = new GameController(mockManager);

            // Act
            var result = controller.CreateGame() as ExceptionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GettingExistingGame_ShouldReturnGameData()
        {
            // Arrange
            var mockManager = Substitute.For<IDataManager>();
            var gameId = Guid.NewGuid();
            var mockGameData = Substitute.For<ITicTacToeData>();
            mockGameData.Id.Returns(gameId);
            mockGameData.CurrentPlayer.Returns(Player.O);   // non-default value to make sure we're accessing *this* game's data
            var gameResult = GameResult.Unfinished;
            mockGameData.Result.Returns(gameResult);
            var board = Enumerable.Repeat(CellState.Empty, 9).ToList();
            mockGameData.Board.Returns(board);
            mockManager.GetGameData(gameId).Returns(mockGameData);
            var controller = new GameController(mockManager);

            // Act
            var result = controller.GetGame(gameId) as OkNegotiatedContentResult<JObject>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(gameId.ToString(), result.Content["Id"].ToString());
            Assert.AreEqual("O", result.Content["CurrentPlayer"].ToString());
            Assert.AreEqual("Unfinished", result.Content["Result"].ToString());
            Assert.AreEqual(board.Count, result.Content["Board"].Count());
            Assert.IsTrue(result.Content["Board"].All(cellState => cellState.ToString() == "Empty"));
        }

        [TestMethod]
        public void GettingNonexistentGame_ShouldReturnNotFound()
        {
            // Arrange
            var mockManager = Substitute.For<IDataManager>();
            var controller = new GameController(mockManager);

            // Act
            var result = controller.GetGame(Guid.NewGuid()) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        // TODO - getting existing game should return 500 on DB error

        // TODO - MakeMove() tests
    }

    class MockDbException : DbException
    {
    }
}
