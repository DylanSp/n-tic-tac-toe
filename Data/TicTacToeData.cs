using System;

namespace Data
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
        WaitingForMove,
        GameFinished,   // returned upon making final move
        GameAlreadyOver // returned when move is made in an already completed game
    }

    [Serializable]
    public class TicTacToeData
    {
        public Player CurrentPlayer { get; set; }

        public GameResult Result { get; set; }

        public CellState[] Board { get; set; }
    }
}
