using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestTicTacToeGame
    {
        [TestMethod]
        public void NewGame_ShouldStartWithPlayerX()
        {
            var game = new GameManager();
            Assert.AreEqual(Player.X, game.GameData.CurrentPlayer);
        }

        [TestMethod]
        public void NewGame_ShouldStartWithUnfinishedGameResult()
        {
            var game = new GameManager();
            Assert.AreEqual(GameResult.Unfinished, game.GameData.Result);
        }

        [TestMethod]
        public void NewGame_ShouldStartWithEmptyBoard()
        {
            var game = new GameManager();
            Assert.IsTrue(game.GameData.Board.All(cell => cell == CellState.Empty));
        }

        [TestMethod]
        public void MoveToFilledSquare_ShouldReturnError()
        {
            var game = new GameManager();
            game.MakeMove(0);
            var result = game.MakeMove(0);
            Assert.AreEqual(MoveResult.CellFilled, result);
        }

        [TestMethod]
        public void XMoveToEmptySquare_ShouldFillCellWithX()
        {
            var cellNum = 0;
            var game = new GameManager();
            var result = game.MakeMove(cellNum);
            Assert.AreEqual(MoveResult.WaitingForMove, result); // should this be in this test, or a separate one?
            Assert.AreEqual(CellState.X, game.GameData.Board[cellNum]);
        }

        [TestMethod]
        public void OMoveToEmptySquare_ShouldFillCellWithO()
        {
            var xCellNum = 0;
            var oCellNum = 1;
            var game = new GameManager();
            game.MakeMove(xCellNum);
            var result = game.MakeMove(oCellNum);
            Assert.AreEqual(MoveResult.WaitingForMove, result); // should this be in this test, or a separate one?
            Assert.AreEqual(CellState.O, game.GameData.Board[oCellNum]);
        }

        [TestMethod]
        public void SuccessfulXMove_ShouldMakeOCurrentPlayer()
        {
            var game = new GameManager();
            game.MakeMove(0);
            Assert.AreEqual(Player.O, game.GameData.CurrentPlayer);
        }

        [TestMethod]
        public void SuccessfulOMove_ShouldMakeXCurrentPlayer()
        {
            var game = new GameManager();
            game.MakeMove(0);
            game.MakeMove(1);
            Assert.AreEqual(Player.X, game.GameData.CurrentPlayer);
        }

        [TestMethod]
        public void XWinningSequence_ShouldShowXAsWinningPlayer()
        {
            var game = new GameManager();
            var results = new List<MoveResult>();
            var moveSequence = new int[] {0, 3, 1, 4, 2};
            foreach (var move in moveSequence)
            {
                var result = game.MakeMove(move);
                results.Add(result);
            }
            Assert.AreEqual(MoveResult.GameFinished, results.Last());
            Assert.AreEqual(GameResult.XWon, game.GameData.Result);
        }

        [TestMethod]
        public void OWinningSequence_ShouldShowOAsWinningPlayer()
        {
            var game = new GameManager();
            var results = new List<MoveResult>();
            var moveSequence = new int[] { 0, 3, 1, 4, 7, 5 };
            foreach (var move in moveSequence)
            {
                var result = game.MakeMove(move);
                results.Add(result);
            }
            Assert.AreEqual(MoveResult.GameFinished, results.Last());
            Assert.AreEqual(GameResult.OWon, game.GameData.Result);
        }

        [TestMethod]
        public void MoveInCompletedGame_ShouldReturnError()
        {
            var game = new GameManager();
            var results = new List<MoveResult>();
            var moveSequence = new int[] { 0, 3, 1, 4, 2, 8};
            foreach (var move in moveSequence)
            {
                var result = game.MakeMove(move);
                results.Add(result);
            }
            Assert.AreEqual(MoveResult.GameAlreadyOver, results.Last());
        }

        [TestMethod]
        public void GameDrawingSequence_ShouldShowDrawnGame()
        {
            var game = new GameManager();
            var results = new List<MoveResult>();
            var moveSequence = new int[] { 0, 3, 1, 4, 5, 2, 6, 7, 8 };
            foreach (var move in moveSequence)
            {
                var result = game.MakeMove(move);
                results.Add(result);
            }
            Assert.AreEqual(GameResult.Drawn, game.GameData.Result);
        }
    }
}
