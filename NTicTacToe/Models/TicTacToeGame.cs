
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

    public class TicTacToeGame
    {
        public Player CurrentPlayer { get; }

        public GameResult Result { get; }

        public CellState[] Board { get; }

        public TicTacToeGame()
        {
            CurrentPlayer = Player.X;
            Result = GameResult.Unfinished;
            Board = Enumerable.Repeat(CellState.Empty, 9).ToArray();
        }
    }
}