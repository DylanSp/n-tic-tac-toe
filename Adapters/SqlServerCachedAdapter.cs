using Data;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapters
{
    public class SqlServerCachedAdapter : IGenericDataAdapter<ITicTacToeData>
    {
        private IGenericDataAdapter<ITicTacToeData> PrimaryDataSource;
        private IGenericDataAdapter<ITicTacToeData> Cache;

        public SqlServerCachedAdapter(IGenericDataAdapter<ITicTacToeData> primaryDataSource, IGenericDataAdapter<ITicTacToeData> cache)
        {
            PrimaryDataSource = primaryDataSource;
            Cache = cache;
        }

        public void Delete(Guid id)
        {
            Cache.Delete(id);
            PrimaryDataSource.Delete(id);
        }

        public (bool, ITicTacToeData) Read(Guid id)
        {
            var (presentInCache, cacheData) = Cache.Read(id);

            if (presentInCache)
            {
                return (true, cacheData);
            }

            var (presentInPrimary, primaryData) = PrimaryDataSource.Read(id);

            if (presentInPrimary)
            {
                Cache.Save(primaryData);
                return (true, primaryData);
            }
            else
            {
                return (false, new EmptyTicTacToeData());
            }
        }

        public void Save(ITicTacToeData newData)
        {
            Cache.Save(newData);
            PrimaryDataSource.Save(newData);
        }
    }
}
