using System;
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
    }
}
