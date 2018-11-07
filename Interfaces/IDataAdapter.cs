namespace Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IDataAdapter<T> where T : IEntity
    {
        T Read(int id);
        int Create();
        void Update(T newData);
        void Delete(int id);
    }
}
