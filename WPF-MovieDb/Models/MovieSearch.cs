using MovieDB.SharedModels;
using System.Collections.ObjectModel;
using WPF_MovieDb.Models.Interfaces;
using WPF_MovieDb.Models.Items;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Models;


public class MovieSearch : IMovieSearch
{
    private readonly IMovieAPIService movieAPIService;
    private readonly IPersonAPIService personAPIService;

    public MovieSearch(IMovieAPIService movieAPIService, IPersonAPIService personAPIService)
    {
        this.movieAPIService = movieAPIService;
        this.personAPIService = personAPIService;
    }

    public List<Movie> GetMovieList(string input, string searchBy, string rank, bool invert, ObservableCollection<LabeledCheckBox> genreCheckBoxes)
    {
        List<Movie> movieList = new();

        // Handel search
        switch (searchBy)
        {
            case "movie name":
                movieList = SearchMovieNames(input);
                break;

            case "persons contributed":
                movieList = SearchPersonNames(input);
                break;

            default:
                throw new Exception("GetMovieList didn't get a valid search mode.");
        }

        // Handel ranking
        switch (rank)
        {
            case "alphabetically":
                movieList = movieList.OrderBy(x => x.Name).ToList();
                break;

            case "by rating":
                movieList = movieList.OrderBy(x => x.Rating).ToList();
                break;

            default:
                throw new Exception("GetMovieList didn't get a valid search mode.");
        }

        // Sorts out movies with none of the selected genres
        movieList = FilterForGenre(movieList, genreCheckBoxes);

        // Invert list if needed
        if (invert)
            movieList.Reverse();

        return movieList;
    }

    public List<Movie> SearchMovieNames(string input)
    {
        List<Movie> moviesFound = new();

        if (input == null)
            input = "";

        foreach (Movie movie in movieAPIService.GetAll())
        {
            if (movie.Name.ToLower().Contains(input.ToLower()))
                moviesFound.Add(movie);
        }
        return moviesFound;
    }

    List<Movie> FilterForGenre(List<Movie> movieList, ObservableCollection<LabeledCheckBox> genreCheckBoxes)
    {
        // Convert genreCheckBoxes to list of genres that are selected
        List<Genres> selectedGenres = new();

        foreach (var item in genreCheckBoxes)
            if (item.IsChecked)
            {
                Genres genre = (Genres)Enum.Parse(typeof(Genres), item.Text, true);
                selectedGenres.Add(genre);
            }

        // Excludes all movies from moviesToReturn that have a genre that is listed in selectedGenres
        List<Movie> moviesToReturn = new();

        foreach (var movie in movieList)
        {
            if (movie.Genres.Count == 0)
            {  
                moviesToReturn.Add(movie);
                continue;
            }

            foreach (Genres genre in movie.Genres)
                if (!selectedGenres.Contains(genre))
                {
                    moviesToReturn.Add(movie);
                    break;
                }
        }

        return moviesToReturn;
    }

    List<Movie> SearchPersonNames(string input)
    {
        List<Movie> moviesFound = [];

        if (input == null)
            input = "";

        foreach (var movie in movieAPIService.GetAll())
        {
            List<int> ids = [];
            ids.AddRange(movie.Star_IDs);
            ids.AddRange(movie.Director_IDs); // ids = all ids of persons in the movie.

            input = input.ToLower();

            foreach (var person in personAPIService.GetAll())
            {
                if (ids.Contains(person.Id) && (person.FirstName + " " + person.LastName).ToLower().Contains(input) && !moviesFound.Contains(movie))
                    moviesFound.Add(movie);
            }
        }
        return moviesFound;
    }
}
