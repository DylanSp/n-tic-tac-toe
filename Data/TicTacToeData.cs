using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public class TicTacToeData : IEntity
    {
        public int Id { get; set; }

        public Player CurrentPlayer { get; set; }

        public GameResult Result { get; set; }

        public List<CellState> Board { get; set; }

        public TicTacToeData ()
        {
            Board = new List<CellState>();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as TicTacToeData;
            var boardsAreEqual = (Board == null && other.Board == null) || Board.SequenceEqual(other.Board);
            return Id.Equals(other.Id)
                && CurrentPlayer.Equals(other.CurrentPlayer)
                && Result.Equals(other.Result)
                && boardsAreEqual;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
