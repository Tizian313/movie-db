using MovieDB.SharedModels;
using YourMovieDB.Models;

namespace YourMovieDB.OperationContracts.Interfaces;


public interface IMenu
{
    void DeletePersonMenu();
    void ListByGenre(Genres genre);
    void ListMoviesRanked(RankingMethods method);
    void MainMenu();
    void SearchByGenre();
    void SearchByString(SearchMethod method);
    void SearchMovies();
    void ShowMovie(Movie movie);
}