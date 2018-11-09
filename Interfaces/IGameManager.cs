using Data;
using Types;

namespace Interfaces
{
    public interface IGameManager
    {
        ITicTacToeData GameData { get; }
        MoveResult MakeMove(int cellNum);
    }
}
