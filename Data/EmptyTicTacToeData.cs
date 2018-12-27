using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace Data
{
    public class EmptyTicTacToeData : IEntity, ITicTacToeData
    {
        public Guid Id
        {
            get
            {
                return Guid.Empty;
            }

            set
            {
                // intentional no-op
            }
        }

        public List<CellState> Board
        {
            get
            {
                return new List<CellState>();
            }

            set
            {
                // intentional no-op
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return Player.X;
            }

            set
            {
                // intentional no-op
            }
        }

        public GameResult Result
        {
            get
            {
                return GameResult.Unfinished;
            }

            set
            {
                // intentional no-op
            }
        }
    }
}
