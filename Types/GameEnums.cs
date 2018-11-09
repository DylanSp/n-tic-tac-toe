using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Types
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
}
