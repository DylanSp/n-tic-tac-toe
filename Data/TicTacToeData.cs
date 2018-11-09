using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Types;

namespace Data
{
    [Serializable]
    public class TicTacToeData : IEntity, ITicTacToeData
    {
        public Guid Id { get; set; }

        public Player CurrentPlayer { get; set; }

        public GameResult Result { get; set; }

        public List<CellState> Board { get; set; }

        public TicTacToeData ()
        {
            Id = Guid.NewGuid();
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
