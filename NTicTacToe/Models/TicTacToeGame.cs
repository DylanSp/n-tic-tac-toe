// Represents a tic-tac-toe game's state
// presents an immutable API, with MakeMove() returning a new game state and maintaining internal consistency
// port of TypeScript tic-tac-toe game class from https://github.com/DylanSp/tic-tac-toe-react/blob/master/src/TicTacToeGame.ts

using System;
using System.Linq;

namespace NTicTacToe.Models
{
    public enum Player
    {
        X,
        O
    }

    public enum GameResult
    {
        XWon,
        OWon,
        Drawn,
        Unfinished
    }

    public enum CellState
    {
        X,
        O,
        Empty
    }

    public enum MoveResult
    {
        CellFilled,
        WaitingForMove
    }

    public class TicTacToeGame
    {
        public Player CurrentPlayer { get; private set; }

        public GameResult Result { get; private set; }

        public CellState[] Board { get; private set; }

        public TicTacToeGame()
        {
            CurrentPlayer = Player.X;
            Result = GameResult.Unfinished;
            Board = Enumerable.Repeat(CellState.Empty, 9).ToArray();
        }

        public (MoveResult result, TicTacToeGame updatedGame) MakeMove(int cellNum)
        {
            if (cellNum < 0 || cellNum > 8)
            {
                throw new Exception("Cell number is outside of the 0-8 range!");
            }

            var updatedGame = new TicTacToeGame();
            updatedGame.CurrentPlayer = this.CurrentPlayer;
            updatedGame.Result = this.Result;
            Array.Copy(this.Board, updatedGame.Board, this.Board.Length);

            if (updatedGame.Board[cellNum] != CellState.Empty)
            {
                return (MoveResult.CellFilled, updatedGame);
            }

            if (updatedGame.CurrentPlayer == Player.X)
            {
                updatedGame.Board[cellNum] = CellState.X;
                updatedGame.CurrentPlayer = Player.O;
            }
            else
            {
                updatedGame.Board[cellNum] = CellState.O;
                updatedGame.CurrentPlayer = Player.X;
            }
            
            return (MoveResult.WaitingForMove, updatedGame);
        }
    }
}