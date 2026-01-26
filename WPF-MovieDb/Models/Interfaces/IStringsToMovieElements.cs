using MovieDB.SharedModels;

namespace WPF_MovieDb.Models.Interfaces;


public interface IStringsToMovieElements
{
    (List<Genres> genres, string errorMessage) GetGenres(string genreText);
    (string name, string errorMessage) GetName(string name);
    (List<Person> personList, bool errorHasOccured) GetPersons(string personText);
    (float rating, string errorMessage) GetRating(string ratingText);
}
