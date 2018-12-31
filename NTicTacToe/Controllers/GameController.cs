using Interfaces;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web.Http;

namespace NTicTacToe.Controllers
{
    [RoutePrefix("api/v0.1")]
    public class GameController : ApiController
    {
        public IDataManager Manager { get; private set; }

        public GameController(IDataManager dataManager)
        {
            Manager = dataManager;
        }

        [HttpGet]
        [Route("games")]
        public IHttpActionResult GetAllGames()
        {
            var allGames = Manager.GetAllGamesData();
            var allGameIds = allGames.Select(game => game.Id);
            var json = JToken.FromObject(allGameIds);
            return Ok(json);
        }
    }
}