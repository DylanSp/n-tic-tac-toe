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

        public RedisAdapter(string connectionString)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            RedisConnection = ConnectionMultiplexer.Connect(connectionString);

            Serializer = new Serializer();
        }

        public void Delete(Guid id)
        {
            var db = RedisConnection.GetDatabase();
            db.KeyDelete(id.ToString());
        }

        public ITicTacToeData Read(Guid id)
        {
            var db = RedisConnection.GetDatabase();
            var gameData = db.StringGet(id.ToString());
            return Serializer.DeserializeGameData(gameData);
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
