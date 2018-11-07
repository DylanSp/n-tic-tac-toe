using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTicTacToe.Models;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestTicTacToeGame
    {
        [TestMethod]
        public void NewGame_ShouldStartWithPlayerX()
        {
            var game = new TicTacToeGame();
            Assert.AreEqual(Player.X, game.CurrentPlayer);
        }

        [TestMethod]
        public void NewGame_ShouldStartWithUnfinishedGameResult()
        {
            var game = new TicTacToeGame();
            Assert.AreEqual(GameResult.Unfinished, game.Result);
        }

        [TestMethod]
        public void NewGame_ShouldStartWithEmptyBoard()
        {
            var game = new TicTacToeGame();
            Assert.IsTrue(game.Board.All(cell => cell == CellState.Empty));
        }

        [TestMethod]
        public void MoveToFilledSquare_ShouldReturnError()
        {
            var game0 = new TicTacToeGame();
            var (_, game1) = game0.MakeMove(0);
            var (result, game2) = game1.MakeMove(0);
            Assert.AreEqual(MoveResult.CellFilled, result);
        }

        [TestMethod]
        public void XMoveToEmptySquare_ShouldFillCellWithX()
        {
            var cellNum = 0;
            var game0 = new TicTacToeGame();
            var (result, game1) = game0.MakeMove(cellNum);
            Assert.AreEqual(MoveResult.WaitingForMove, result); // should this be in this test, or a separate one?
            Assert.AreEqual(CellState.X, game1.Board[cellNum]);
        }

        [TestMethod]
        public void OMoveToEmptySquare_ShouldFillCellWithO()
        {
            var xCellNum = 0;
            var oCellNum = 1;
            var game0 = new TicTacToeGame();
            var (_, game1) = game0.MakeMove(xCellNum);
            var (result, game2) = game1.MakeMove(oCellNum);
            Assert.AreEqual(MoveResult.WaitingForMove, result); // should this be in this test, or a separate one?
            Assert.AreEqual(CellState.O, game2.Board[oCellNum]);
        }

        [TestMethod]
        public void SuccessfulXMove_ShouldMakeOCurrentPlayer()
        {
            var game0 = new TicTacToeGame();
            var (_, game1) = game0.MakeMove(0);
            Assert.AreEqual(Player.O, game1.CurrentPlayer);
        }

        [TestMethod]
        public void SuccessfulOMove_ShouldMakeXCurrentPlayer()
        {
            var game0 = new TicTacToeGame();
            var (_, game1) = game0.MakeMove(0);
            var (_, game2) = game1.MakeMove(1);
            Assert.AreEqual(Player.X, game2.CurrentPlayer);
        }

        [TestMethod]
        public void XWinningSequence_ShouldShowXAsWinningPlayer()
        {
            var games = new List<TicTacToeGame>
            {
                new TicTacToeGame()
            };
            var results = new List<MoveResult>();
            var moveSequence = new int[] {0, 3, 1, 4, 2};
            foreach (var move in moveSequence)
            {
                var (result, game) = games.Last().MakeMove(move);
                results.Add(result);
                games.Add(game);
            }
            Assert.AreEqual(MoveResult.GameFinished, results.Last());
            Assert.AreEqual(GameResult.XWon, games.Last().Result);
        }

        [TestMethod]
        public void OWinningSequence_ShouldShowOAsWinningPlayer()
        {
            var games = new List<TicTacToeGame>
            {
                new TicTacToeGame()
            };
            var results = new List<MoveResult>();
            var moveSequence = new int[] { 0, 3, 1, 4, 7, 5 };
            foreach (var move in moveSequence)
            {
                var (result, game) = games.Last().MakeMove(move);
                results.Add(result);
                games.Add(game);
            }
            Assert.AreEqual(MoveResult.GameFinished, results.Last());
            Assert.AreEqual(GameResult.OWon, games.Last().Result);
        }

        [TestMethod]
        public void MoveInCompletedGame_ShouldReturnError()
        {
            var games = new List<TicTacToeGame>
            {
                new TicTacToeGame()
            };
            var results = new List<MoveResult>();
            var moveSequence = new int[] { 0, 3, 1, 4, 2, 8};
            foreach (var move in moveSequence)
            {
                var (result, game) = games.Last().MakeMove(move);
                results.Add(result);
                games.Add(game);
            }
            Assert.AreEqual(MoveResult.GameAlreadyOver, results.Last());
        }

        [TestMethod]
        public void GameDrawingSequence_ShouldShowDrawnGame()
        {
            var games = new List<TicTacToeGame>
            {
                new TicTacToeGame()
            };
            var results = new List<MoveResult>();
            var moveSequence = new int[] { 0, 3, 1, 4, 5, 2, 6, 7, 8 };
            foreach (var move in moveSequence)
            {
                var (result, game) = games.Last().MakeMove(move);
                results.Add(result);
                games.Add(game);
            }
            Assert.AreEqual(GameResult.Drawn, games.Last().Result);
        }
    }
}
