using FluentAssertions;
using Moq;
using MovieDB.SharedModels;
using WPF_MovieDb.Models;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Test;


[TestClass]
public class StringsToMovieElementsTest
{
    [TestMethod]
    public void SinglePerson()
    {
        List<Person> allPersons = new()
        {
            new() {FirstName = "Mattis", LastName = "Luncz"},
            new() {FirstName = "Tizian", LastName = "Wittstadt"},
            new() {FirstName = "Cristian", LastName = "Carcassco"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis luncz")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian wittstadt")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian carcassco")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);

        var result = stme.GetPersons("Mattis Luncz").personList;


        List<Person> expectedResult = new()
        {
            new() {FirstName = "Mattis", LastName = "Luncz"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void MultiblePersons()
    {
        List<Person> allPersons = new()
        {
            new() {FirstName = "Mattis", LastName = "Luncz"},
            new() {FirstName = "Tizian", LastName = "Wittstadt"},
            new() {FirstName = "Cristian", LastName = "Carcassco"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis luncz")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian wittstadt")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian carcassco")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);
        var result = stme.GetPersons("Mattis Luncz,tizian Wittstadt").personList;

        List<Person> expectedResult = new()
        {
            new() {FirstName = "Mattis", LastName = "Luncz"},
            new() {FirstName = "Tizian", LastName = "Wittstadt"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void NoValidPerson()
    {
        var allPersons = new List<Person>()
        {
            new() {FirstName = "Mattis", LastName = "Luncz"},
            new() {FirstName = "Tizian", LastName = "Wittstadt"},
            new() {FirstName = "Cristian", LastName = "Carcassco"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis luncz")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian wittstadt")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian carcassco")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);
        var result = stme.GetPersons("Tizian Wittstadt, Mattis Benjamin Luncz");


        List<Person> expectedPersonListResult = new()
        {
            new() {FirstName = "Mattis Benjamin", LastName = "Luncz", DateOfBirth = DateTime.Parse("01/01/1900")},
        };

        result.personList.Should().BeEquivalentTo(expectedPersonListResult);
        result.errorHasOccured.Should().BeTrue();
    }
}
