using Microsoft.EntityFrameworkCore;
using MovieDB.SharedModels;

namespace YourMovieDB.Models;


public class UsedData
{
    public DbSet<Person> AllPersons { get; set; }
    public DbSet<Movie> AllMovies { get; set; }

    public UsedData(DbSet<Person> allPersons, DbSet<Movie> allMovies)
    { 
        AllMovies = allMovies;
        AllPersons = allPersons;
    }
}
