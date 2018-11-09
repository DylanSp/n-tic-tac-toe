using Data;
using Interfaces;
using Types;

namespace Managers
{
    public class DataManager
    {
        public DataManager(IGameManager gameManager, IGenericDataAdapter<ITicTacToeData> adapter)
        {
            GameManager = gameManager;
            Adapter = adapter;
        }

        public IGameManager GameManager { get; private set; }
        private IGenericDataAdapter<ITicTacToeData> Adapter { get; set; }

        // don't want to do this in constructor, because CreateOrUpdate() can throw
        public void CreateAndSaveGame()
        {
            GameManager = new GameManager();
            Adapter.Save(GameManager.GameData);
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
