using Interfaces;
using System;
using System.Collections.Generic;
using Types;

namespace Data
{
    public interface ITicTacToeData : IEntity
    {
        List<CellState> Board { get; set; }
        Player CurrentPlayer { get; set; }
        GameResult Result { get; set; }
    }
}