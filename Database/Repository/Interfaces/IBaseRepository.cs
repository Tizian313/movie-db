using MovieDB.SharedModels.Interfaces;

namespace Database.Repository.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class, IIdentifications, new()
    {
        void Add(TEntity entry);
        TEntity? Get(int id);
        List<TEntity> GetAll();
        void Remove(int id);
    }
}