using MovieDB.SharedModels;
using WPF_MovieDb.Models.Interfaces;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Models;


public class GenerateInitialText : IGenerateInitialText
{
    private readonly IPersonAPIService personApiService;

    public GenerateInitialText(IPersonAPIService personApiService)
    {
        this.personApiService = personApiService;
    }

    public string ForGenres(Movie movie)
    {
        string genreText = string.Empty;

        if (movie.Genres.Count > 0)
        {
            foreach (Genres genre in movie.Genres)
                genreText += genre.ToString() + ", ";

            genreText = genreText.Remove(genreText.Length - 2);
        }

        return genreText;
    }

    public string ForDirector(Movie movie)
    {
        return ForPersons(movie, isDirector: true);
    }

    public string ForActor(Movie movie)
    {
        return ForPersons(movie, isDirector: false);
    }

    string ForPersons(Movie movie, bool isDirector)
    {
        string personText = string.Empty;

        // Depended on the "mode" selected, a different list is used in the foreach-loop.
        List<int> personIdList;

        if (isDirector)
            personIdList = movie.Director_IDs;
        else
            personIdList = movie.Star_IDs;


        foreach (int personId in personIdList)
        {
            Person? person = personApiService.Get(personId);

            if (person != null)
                personText += person.FirstName + " " + person.LastName + ", ";
        }


        if (personText.Length > 2)
            personText = personText.Remove(personText.Length - 2);

        return personText;

    }

    public string ForRating(Movie movie)
    {
        return (movie.Rating * 10).ToString();
    }
}
