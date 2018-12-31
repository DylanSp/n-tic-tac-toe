using System.Collections.Generic;
using Types;

namespace Interfaces
{
    public interface ITicTacToeData : IEntity
    {
        List<CellState> Board { get; set; }
        Player CurrentPlayer { get; set; }
        GameResult Result { get; set; }
    }
}