using MovieDB.SharedModels;

namespace WPF_MovieDb.Models.Interfaces;


public interface IGenerateInitialText
{
    string ForActor(Movie movie);
    string ForDirector(Movie movie);
    string ForGenres(Movie movie);
    string ForRating(Movie movie);
}
