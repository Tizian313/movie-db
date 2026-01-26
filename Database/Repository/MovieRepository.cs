using Database.Repository.Interfaces;
using MovieDB.SharedModels;

namespace Database.Repository;


public class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    protected readonly YourMovieDBContext context;


    public MovieRepository(YourMovieDBContext context) : base(context)
    {
        this.context = context;
    }
}
