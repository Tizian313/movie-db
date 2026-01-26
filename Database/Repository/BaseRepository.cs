using Microsoft.EntityFrameworkCore;
using MovieDB.SharedModels.Interfaces;

namespace Database.Repository;


public class BaseRepository<TEntity> where TEntity : class, IIdentifications, new()
{
    protected readonly YourMovieDBContext context;


    public BaseRepository(YourMovieDBContext context)
    {
        this.context = context;
    }


    public TEntity? Get(int id)
    {
        return context.Set<TEntity>().FirstOrDefault(q => q.Id == id);
    }

    public List<TEntity> GetAll()
    {
        return context.Set<TEntity>().ToList();
    }

    public void Add(TEntity entry)
    {
        context.Set<TEntity>().Add(entry);
        context.SaveChanges();
    }

    public void Remove(int id)
    {
        context.Set<TEntity>().Where(q => q.Id == id).ExecuteDelete();
        context.SaveChanges();
    }
}
