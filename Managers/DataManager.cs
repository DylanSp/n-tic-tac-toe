using Data;
using Interfaces;
using System;
using Types;

namespace Managers
{
    public class DataManager : IDataManager
    {
        private IGameManager GameManager { get; set; }
        private IGenericDataAdapter<ITicTacToeData> Adapter { get; set; }

        public DataManager(IGameManager gameManager, IGenericDataAdapter<ITicTacToeData> adapter)
        {
            GameManager = gameManager;
            Adapter = adapter;
        }

        public void CreateAndSaveGame()
        {
            GameManager.ResetGame();
            Adapter.Save(GameManager.GameData);
        }

        public ITicTacToeData GetGameData()
        {
            var (readSuccessfully, gameData) = Adapter.Read(GameManager.GameData.Id);
            if (readSuccessfully)
            {
                return gameData;
            }
            else
            {
                // TODO - really throw an exception here?
                throw new Exception("Game doesn't exist!");
            }
        }

        public MoveResult AttemptAndSaveMove(int cellNum)
        {
            var result = GameManager.MakeMove(cellNum);
            
            // if move was successful, game state is changed, save it via adapter
            if (result == MoveResult.WaitingForMove || result == MoveResult.GameFinished)
            {
                Adapter.Save(GameManager.GameData);
            }

            return result;
        }
    }
}
