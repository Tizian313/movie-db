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
public class PersonControllerTest
{
    private readonly Mock<IPersonRepository> repoMock;
    private readonly PersonController controller;


    public PersonControllerTest()
    {
        var nullLogger = NullLogger<PersonController>.Instance;

        this.repoMock = new Mock<IPersonRepository>();
        this.controller = new PersonController(nullLogger, repoMock.Object);
    }


    // Tests if GetAll returns all People.
    [TestMethod]
    public void GetAllTest()
    {
        //string firstName, string lastName, DateTime dateOfBirth, int creatorId
        List<Person> people = new() {
            new("", "", new(), 0),
            new("", "", new(), 0),
            new("", "", new(), 0)
    };

        repoMock.Setup(x => x.GetAll())
                .Returns(people);

        IEnumerable<Person> result = controller.GetAll();

        result.Count()
              .Should()
              .Be(3);
    }

    [TestMethod]
    public void AddTest()
    {
        List<Person> people = new() {
            new("", "", new(), 0),
            new("", "", new(), 0),
            new("", "", new(), 0)
        };

        Person personToAdd = new("This is the person to add!", "", new(), 0);

        // Mock IMovieRepository
        repoMock.Setup(x => x.Add(It.IsAny<Person>()))
                .Callback<Person>(m => people.Add(m));

        repoMock.Setup(x => x.GetAll())
                .Returns(() => people);

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
        controller.Add(personToAdd);

        IEnumerable<Person> result = controller.GetAll();

        result.Last()
              .Should()
              .Be(personToAdd);
    }
}
