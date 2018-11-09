using Data;
using Interfaces;
using Types;

namespace Managers
{
    public interface IDataManager
    {
        MoveResult AttemptAndSaveMove(int cellNum);
        void CreateAndSaveGame();
        ITicTacToeData GetGameData();
    }
}