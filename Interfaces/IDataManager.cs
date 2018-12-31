using System;
using System.Collections.Generic;
using Types;

namespace Interfaces
{
    public interface IDataManager
    {
        MoveResult AttemptAndSaveMove(Guid gameId, int cellNum);
        Guid CreateAndSaveGame();
        ITicTacToeData GetGameData(Guid gameId);
        IEnumerable<ITicTacToeData> GetAllGamesData();
    }
}