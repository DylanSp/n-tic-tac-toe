using System;

namespace Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }


    public interface IGenericDataAdapter<T> where T : IEntity
    {
        void Save(T newData);
        (bool, T) Read(Guid id);
        void Delete(Guid id);
    }
}
