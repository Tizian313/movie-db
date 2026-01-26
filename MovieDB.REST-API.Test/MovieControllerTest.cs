using Database.Repository.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MovieDB.REST_API.Controllers;
using MovieDB.SharedModels;
using System.Security.Claims;

namespace MovieDB.REST_API.Test;


[TestClass]
public class MovieControllerTest
{
    private readonly Mock<IMovieRepository> repoMock;
    private readonly MovieController controller;


    public MovieControllerTest()
    {
        var nullLogger = NullLogger<MovieController>.Instance;

        this.repoMock = new Mock<IMovieRepository>();
        this.controller = new MovieController(nullLogger, repoMock.Object);
    }


    // Tests if GetAll returns all Movies.
    [TestMethod]
    public void GetAllTest()
    {
        List<Movie> movies = new() {
            new("", new(), new(), new(), 0.0f, 0),
            new("", new(), new(), new(), 0.0f, 0),
            new("", new(), new(), new(), 0.0f, 0)
        };

        repoMock.Setup(x => x.GetAll())
                .Returns(movies);

        IEnumerable<Movie> result = controller.GetAll();

        result.Count()
              .Should()
              .Be(3);
    }

    [TestMethod]
    public void AddTest()
    {
        List<Movie> movies = new() {
            new("", new(), new(), new(), 0.0f, 0),
            new("", new(), new(), new(), 0.0f, 0),
            new("", new(), new(), new(), 0.0f, 0)
        };

        Movie movieToAdd = new("This is the movie to add.", new(), new(), new(), 0.0f, 0);

        // Mock IMovieRepository
        repoMock.Setup(x => x.Add(It.IsAny<Movie>()))
                .Callback<Movie>(m => movies.Add(m));

        repoMock.Setup(x => x.GetAll())
                .Returns(() => movies);

        // Mock a user with a claim
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Test adding a movie
        controller.Add(movieToAdd);

        IEnumerable<Movie> result = controller.GetAll();

        result.Last()
              .Should()
              .Be(movieToAdd);
    }
}