using Microsoft.EntityFrameworkCore;
using MovieDB.SharedModels;

namespace Database;


public class YourMovieDBContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<User> Users { get; set; }


    public YourMovieDBContext(DbContextOptions<YourMovieDBContext> dbContextOptions) : base(dbContextOptions) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public YourMovieDBContext() { }

    // Uncomment to be able to add a migration.
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=MovieDatabase; Trusted_Connection=True");
    // }
}
