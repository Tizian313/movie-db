using FluentAssertions;
using Moq;
using MovieDB.SharedModels;
using WPF_MovieDb.Models;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Test;


[TestClass]
public class GenerateInitialTextTest
{
    [TestMethod]
    public void ForPersonTest()
    {
        Movie subjectMovie = new() { Star_IDs = [2, 1] };

        List<Person> allPersons = new()
        {
            new() {Id = 0, FirstName = "Mattis", LastName = "TestA"},
            new() {Id = 1, FirstName = "Tizian", LastName = "TestB"},
            new() {Id = 2, FirstName = "Cristian", LastName = "TestC"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get(0)).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get(1)).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get(2)).Returns(allPersons[2]);

        GenerateInitialText gti = new(personRepoMock.Object);

        var result = gti.ForActor(subjectMovie);

        result.Should().BeEquivalentTo("Cristian TestC, Tizian TestB");
    }

    [TestMethod]
    public void ForGenresTest()
    {
        Movie subjectMovie = new() { Genres = [Genres.Romance, Genres.Western, Genres.Scifi] };

        Mock<IPersonAPIService> personRepoMock = new();
        GenerateInitialText gti = new(personRepoMock.Object);

        var result = gti.ForGenres(subjectMovie);

        result.Should().BeEquivalentTo("Romance, Western, Scifi");
    }
}
