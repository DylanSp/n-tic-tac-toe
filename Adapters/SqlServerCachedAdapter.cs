using Data;
using Interfaces;
using System;
using System.Collections.Generic;

namespace Adapters
{
    // TODO - handle errors. swallow errors from cache, let errors from primary bubble up?
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

        public IEnumerable<ITicTacToeData> ReadAll()
        {
            // can't rely on cache having all data, so just go to the primary source
            return PrimaryDataSource.ReadAll();
        }

        public void Save(ITicTacToeData newData)
        {
            // lock here? but can't use await, then, so everything's synchronous
            // we might be ok with that - existing DAL is all synchronous, looks like
            // more importantly, would have to use distributed lock in Docuphase code, which is :(
            Cache.Save(newData);
            PrimaryDataSource.Save(newData);
        }
    }
}
