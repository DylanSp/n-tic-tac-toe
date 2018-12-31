using Interfaces;

namespace Managers
{
    public class GameManagerFactory : IGameManagerFactory
    {
        public IGameManager CreateGameManager(ITicTacToeData gameData)
        {
            return new GameManager(gameData);
        }
    }
}
