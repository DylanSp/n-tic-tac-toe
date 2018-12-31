using Data;
using Interfaces;
using System;
using System.Collections.Generic;
using Types;

namespace Managers
{
    public class DataManager : IDataManager
    {
        private IGameManagerFactory GameManagerFactory { get; set; }
        private IGenericDataAdapter<ITicTacToeData> Adapter { get; set; }

        public DataManager(IGameManagerFactory gameManagerFactory, IGenericDataAdapter<ITicTacToeData> adapter)
        {
            GameManagerFactory = gameManagerFactory;
            Adapter = adapter;
        }

        public Guid CreateAndSaveGame()
        {
            var gameManager = GameManagerFactory.CreateGameManager(new TicTacToeData());    // TODO - should I be explicitly constructing concrete TTTData here?
            gameManager.ResetGame();
            Adapter.Save(gameManager.GameData);
            return gameManager.GameData.Id;
        }

        public ITicTacToeData GetGameData(Guid gameId)
        {
            var (readSuccessfully, gameData) = Adapter.Read(gameId);
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

        public MoveResult AttemptAndSaveMove(Guid gameId, int cellNum)
        {
            var (readSuccessfully, gameData) = Adapter.Read(gameId);
            if (!readSuccessfully)
            {
                // TODO - throw exception? have (bool, MoveResult) return type instead?
                throw new Exception("Game doesn't exist!");
            }

            var gameManager = GameManagerFactory.CreateGameManager(gameData);
            var result = gameManager.MakeMove(cellNum);
            
            // if move was successful, game state is changed, save it via adapter
            if (result == MoveResult.WaitingForMove || result == MoveResult.GameFinished)
            {
                Adapter.Save(gameManager.GameData);
            }

            return result;
        }

        public IEnumerable<ITicTacToeData> GetAllGamesData()
        {
            return Adapter.ReadAll();
        }
    }
}
