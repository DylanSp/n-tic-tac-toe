using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Managers
{
    public class GameManager
    {
        public TicTacToeData GameData { get; private set; }

        public GameManager()
        {
            GameData = new TicTacToeData
            {
                CurrentPlayer = Player.X,
                Result = GameResult.Unfinished,
                Board = Enumerable.Repeat(CellState.Empty, 9).ToList()
            };
        }

        public MoveResult MakeMove(int cellNum)
        {
            if (cellNum < 0 || cellNum > 8)
            {
                throw new Exception("Cell number is outside of the 0-8 range!");
            }

            // check if cell is already filled
            if (GameData.Board[cellNum] != CellState.Empty)
            {
                return MoveResult.CellFilled;
            }

            // check if game is already over
            if (GameData.Result != GameResult.Unfinished)
            {
                return MoveResult.GameAlreadyOver;
            }

            // move is legal => update the board and current player
            if (GameData.CurrentPlayer == Player.X)
            {
                GameData.Board[cellNum] = CellState.X;
                GameData.CurrentPlayer = Player.O;
            }
            else
            {
                GameData.Board[cellNum] = CellState.O;
                GameData.CurrentPlayer = Player.X;
            }

            // check for victory
            UpdateGameResult();
            if (GameData.Result != GameResult.Unfinished)
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
                new List<CellState> { GameData.Board[0], GameData.Board[1], GameData.Board[2] },
                new List<CellState> { GameData.Board[3], GameData.Board[4], GameData.Board[5] },
                new List<CellState> { GameData.Board[6], GameData.Board[7], GameData.Board[8] },

                // columns
                new List<CellState> { GameData.Board[0], GameData.Board[3], GameData.Board[6] },
                new List<CellState> { GameData.Board[1], GameData.Board[4], GameData.Board[7] },
                new List<CellState> { GameData.Board[2], GameData.Board[5], GameData.Board[8] },

                // diagonals
                new List<CellState> { GameData.Board[0], GameData.Board[4], GameData.Board[8] },
                new List<CellState> { GameData.Board[2], GameData.Board[4], GameData.Board[6] }
            };

            foreach (var line in lines)
            {
                if (line.All(cell => cell == CellState.X))
                {
                    GameData.Result = GameResult.XWon;
                    return;
                }
                else if (line.All(cell => cell == CellState.O))
                {
                    GameData.Result = GameResult.OWon;
                    return;
                }
            }

            // check for draw;
            // if all cells are filled but no victory was detected above, game is drawn
            if (GameData.Board.All(cell => cell != CellState.Empty))
            {
                GameData.Result = GameResult.Drawn;
                return;
            }

            GameData.Result = GameResult.Unfinished;
        }
    }
}
