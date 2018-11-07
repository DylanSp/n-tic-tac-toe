// Represents a tic-tac-toe game's state
// presents an immutable API, with MakeMove() returning a new game state and maintaining internal consistency
// port of TypeScript tic-tac-toe game class from https://github.com/DylanSp/tic-tac-toe-react/blob/master/src/TicTacToeGame.ts

using System;
using System.Collections.Generic;
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
        WaitingForMove,
        GameFinished,   // returned upon making final move
        GameAlreadyOver // returned when move is made in an already completed game
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

        public MoveResult MakeMove(int cellNum)
        {
            if (cellNum < 0 || cellNum > 8)
            {
                throw new Exception("Cell number is outside of the 0-8 range!");
            }

            // check if cell is already filled
            if (Board[cellNum] != CellState.Empty)
            {
                return MoveResult.CellFilled;
            }

            // check if game is already over
            if (Result != GameResult.Unfinished)
            {
                return MoveResult.GameAlreadyOver;
            }

            // move is legal => update the board and current player
            if (CurrentPlayer == Player.X)
            {
                Board[cellNum] = CellState.X;
                CurrentPlayer = Player.O;
            }
            else
            {
                Board[cellNum] = CellState.O;
                CurrentPlayer = Player.X;
            }

            // check for victory
            UpdateGameResult();
            if (Result != GameResult.Unfinished)
            {
                return MoveResult.GameFinished;
            }
            
            return MoveResult.WaitingForMove;
        }

        private void UpdateGameResult()
        {
            var lines = new List<List<CellState>>
            {
                // rows
                new List<CellState> {
                    Board[0],
                    Board[1],
                    Board[2]
                },
                new List<CellState> {
                    Board[3],
                    Board[4],
                    Board[5]
                },
                new List<CellState> {
                    Board[6],
                    Board[7],
                    Board[8]
                },

                // columns
                new List<CellState> {
                    Board[0],
                    Board[3],
                    Board[6]
                },
                new List<CellState> {
                    Board[1],
                    Board[4],
                    Board[7]
                },
                new List<CellState> {
                    Board[2],
                    Board[5],
                    Board[8]
                },

                // diagonals
                new List<CellState> {
                    Board[0],
                    Board[4],
                    Board[8]
                },
                new List<CellState> {
                    Board[2],
                    Board[4],
                    Board[6]
                }
            };

            foreach (var line in lines)
            {
                if (line.All(cell => cell == CellState.X))
                {
                    Result = GameResult.XWon;
                    return;
                }
                else if (line.All(cell => cell == CellState.O))
                {
                    Result = GameResult.OWon;
                    return;
                }
            }

            // check for draw;
            // if all cells are filled but no victory was detected above, game is drawn
            if (Board.All(cell => cell != CellState.Empty))
            {
                Result = GameResult.Drawn;
                return;
            }

            Result = GameResult.Unfinished;
        }
    }
}