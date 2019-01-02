using Data;
using Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapters
{
    public class RedisAdapter : IGenericDataAdapter<ITicTacToeData>
    {
        // TODO - make into a persistent context
        private readonly ConnectionMultiplexer RedisConnection;
        private readonly Serializer Serializer;

        public RedisAdapter(ConnectionMultiplexer connection)
        {
            RedisConnection = connection;
            Serializer = new Serializer();
        }

        public void Delete(Guid id)
        {
            var db = RedisConnection.GetDatabase();
            db.KeyDelete(id.ToString());
        }

        public (bool, ITicTacToeData) Read(Guid id)
        {
            var db = RedisConnection.GetDatabase();
            var gameData = db.StringGet(id.ToString());
            if (gameData == RedisValue.Null)
            {
                return (false, new EmptyTicTacToeData());
            }
            else
            {
                return (true, Serializer.DeserializeGameData(gameData));
            }
        }

        public IEnumerable<ITicTacToeData> ReadAll()
        {
            var endpoints = RedisConnection.GetEndPoints();
            var server = RedisConnection.GetServer(endpoints[0]);
            var keys = server.Keys();
            var allGames = new List<ITicTacToeData>();
            var db = RedisConnection.GetDatabase();
            foreach(var key in keys)
            {
                var gameData = db.StringGet(key);
                allGames.Add(Serializer.DeserializeGameData(gameData));
            }
            return allGames;
        }

        public void Save(ITicTacToeData newData)
        {
            var id = newData.Id.ToString();
            var newDataString = Serializer.SerializeGameData(newData);

            var db = RedisConnection.GetDatabase();
            db.StringSet(id, newDataString);
        }
    }
}
