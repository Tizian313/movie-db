using FluentAssertions;
using Moq;
using MovieDB.SharedModels;
using WPF_MovieDb.Models;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Test;


[TestClass]
public class MovieSearchTest
{
    [TestMethod]
    public void FindMovie()
    {
        List<Movie> allMovies = new()
        {
            new() {Name = "Test Movie"},
            new() {Name = "Mattis Mystery"},
            new() {Name = "Mock II: The return"},
        };

        Mock<IMovieAPIService> movieRepoMock = new();
        Mock<IPersonAPIService> personRepoMock = new();
        movieRepoMock.Setup(x => x.GetAll()).Returns(allMovies);

        var movieSearch = new MovieSearch(movieRepoMock.Object, personRepoMock.Object);

        var result = movieSearch.SearchMovieNames("mattis");

        List<Movie> expectedResult = new()
        {
            new() { Name = "Mattis Mystery"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void NoValidMovie()
    {
        List<Movie> allMovies = new()
    {
        new() {Name = "Test Movie"},
        new() {Name = "Mattis Mystery"},
        new() {Name = "Mock II: The return"},
    };

        Mock<IMovieAPIService> movieRepoMock = new();
        Mock<IPersonAPIService> personRepoMock = new();
        movieRepoMock.Setup(x => x.GetAll()).Returns(allMovies);

        var movieSearch = new MovieSearch(movieRepoMock.Object, personRepoMock.Object);


        var result = movieSearch.SearchMovieNames("mattis certainty");

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void EmptyDatabase()
    {
        List<Movie> allMovies = new();

        Mock<IMovieAPIService> movieRepoMock = new();
        Mock<IPersonAPIService> personRepoMock = new();
        movieRepoMock.Setup(x => x.GetAll()).Returns(allMovies);
        var movieSearch = new MovieSearch(movieRepoMock.Object, personRepoMock.Object);

        var result = movieSearch.SearchMovieNames("mattis certainty");

        result.Should().BeEmpty();
    }
}
