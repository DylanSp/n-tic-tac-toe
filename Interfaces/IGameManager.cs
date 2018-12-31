using Types;

namespace Interfaces
{
    public interface IGameManager
    {
        ITicTacToeData GameData { get; }
        void ResetGame();
        MoveResult MakeMove(int cellNum);
    }
}
