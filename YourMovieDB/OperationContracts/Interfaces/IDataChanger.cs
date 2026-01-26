using MovieDB.SharedModels;

namespace YourMovieDB.OperationContracts.Interfaces;


public interface IDataChanger
{
    void CreateMovie();
    Movie CreateMovieEdit(int selection, Movie movie);
    Person? CreatePerson(string invalidName);
    Person? CreatePersonEdit(int selection, Person person);
    void EditMovie(Movie movie);
    string IDtoPersonNames(List<int> input);
    List<int> InputPersons(string input);
}
