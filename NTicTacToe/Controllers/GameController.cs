using Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Http;
using Types;

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
            var json = JArray.FromObject(allGameIds);
            return Ok(json);
        }

        [HttpPost]
        [Route("games")]
        public IHttpActionResult CreateGame()
        {
            try
            {
                var gameId = Manager.CreateAndSaveGame();
                var json = new JObject
                {
                    ["Id"] = gameId
                };
                return Ok(json);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        // TODO - should I just return serialized gameData? (probably only if I'm using a C# frontend)
        [HttpGet]
        [Route("games/{gameId:guid}")]
        public IHttpActionResult GetGame(Guid gameId)
        {
            try
            {
                var gameData = Manager.GetGameData(gameId);
                var json = SerializeGameDataAsJson(gameData);
                return Ok(json);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // TODO - in future, transmit cellNum in body as JSON?
        [HttpPost]
        [Route("games/{gameId:guid}/{cellNum:int}")]
        public IHttpActionResult MakeMove(Guid gameId, int cellNum)
        {
            var moveResult = Manager.AttemptAndSaveMove(gameId, cellNum);
            switch(moveResult)
            {
                // intentional fall-through
                case MoveResult.WaitingForMove:
                case MoveResult.GameFinished:
                    return Ok(SerializeGameDataAsJson(Manager.GetGameData(gameId)));
                case MoveResult.CellFilled:
                    return BadRequest($"Cell {cellNum} already filled");
                case MoveResult.GameAlreadyOver:
                    return BadRequest("Game is already over");
                default:
                    // should never be reached
                    throw new Exception("Unaccounted-for MoveResult");
            }
        }

        private JObject SerializeGameDataAsJson(ITicTacToeData gameData)
        {
            var currentPlayer = gameData.CurrentPlayer == Player.X ? "X" : "O";
            string result;
            switch (gameData.Result)
            {
                case GameResult.Unfinished:
                    result = "Unfinished";
                    break;
                case GameResult.XWon:
                    result = "XVictory";
                    break;
                case GameResult.OWon:
                    result = "OVictory";
                    break;
                case GameResult.Drawn:
                    result = "Drawn";
                    break;
                default:
                    // should never be reached
                    throw new Exception("Unaccounted-for GameResult");
            }

            var jsonBoard = new string[9];
            for (var i = 0; i < jsonBoard.Length; i++)
            {
                switch(gameData.Board[i])
                {
                    case CellState.X:
                        jsonBoard[i] = "X";
                        break;
                    case CellState.O:
                        jsonBoard[i] = "O";
                        break;
                    case CellState.Empty:
                        jsonBoard[i] = "Empty";
                        break;
                    default:
                        // should never be reached
                        throw new Exception("Unaccounted-for CellState");
                }
            }

            var json = new JObject
            {
                ["Id"] = gameData.Id,
                ["CurrentPlayer"] = currentPlayer,
                ["Result"] = result,
                ["Board"] = new JArray(jsonBoard)
            };
            return json;
        }
    }
}