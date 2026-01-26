using Database.Repository.Interfaces;
using YourMovieDB.OperationContracts;
using YourMovieDB.OperationContracts.Interfaces;
using MovieDB.SharedModels;
using Database.Repository;
using Database;

namespace YourMovieDB;


public class Program
{
    private static YourMovieDBContext context = new(new());
    private static IMovieRepository movieRepository = new MovieRepository(context);
    private static IPersonRepository personRepository = new PersonRepository(context);

    private static IDraw draw = new Draw();
    private static IDataChanger dataChanger = new DataChanger(draw, personRepository, movieRepository);
    private static IMenu menu = new Menu(draw, dataChanger, personRepository, movieRepository);
    
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Your Movie Database";

        menu.MainMenu();
        Environment.Exit(0);
    }
}
