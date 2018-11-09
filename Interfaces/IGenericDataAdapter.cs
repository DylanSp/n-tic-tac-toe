using System;

namespace Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }


    public interface IGenericDataAdapter<T> where T : IEntity
    {
        T Read(Guid id);
        void CreateOrUpdate(T newData);
        void Delete(Guid id);
    }
}
