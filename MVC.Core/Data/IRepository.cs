namespace MVC.Core.Data
{
    using System;
    using System.Threading.Tasks;

    public interface IRepository<T, Key> : IReadOnlyRepository<T, Key> where T : class, IEntity<Key> where Key : IConvertible
    {
        T Insert(T entityToInsert);

        T Update(T entityToUpdate);

        void Delete(Key id);

        void Delete(T entityToDelete);

        void Save();

        Task SaveAsync();
    }

    /*
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class, IEntity
    {
        T Insert(T entityToInsert);

        T Update(T entityToUpdate);

        void Delete(object id);

        void Delete(T entityToDelete);

        void Save();

        Task SaveAsync();
    }
    */
}
