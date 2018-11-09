using Data;
using Interfaces;

namespace Managers
{
    class DataManager
    {
        public GameManager GameManager { get; private set; }
        private IDataAdapter<TicTacToeData> Adapter { get; set; }

        // don't want to do this in constructor, because CreateOrUpdate() can throw
        public void CreateAndSaveGame()
        {
            GameManager = new GameManager();
            Adapter.CreateOrUpdate(GameManager.GameData);
        }

        public MoveResult AttemptAndSaveMove(int cellNum)
        {
            var result = GameManager.MakeMove(cellNum);
            
            // if move was successful, game state is changed, save it via adapter
            if (result == MoveResult.WaitingForMove || result == MoveResult.GameFinished)
            {
                Adapter.CreateOrUpdate(GameManager.GameData);
            }

            return result;
        }
    }
}
