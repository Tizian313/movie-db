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
            new() {FirstName = "Mattis", LastName = "Mueller"},
            new() {FirstName = "Tizian", LastName = "Tischer"},
            new() {FirstName = "Cristian", LastName = "Cramer"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis Mueller")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian Tischer")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian Cramer")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);

        var result = stme.GetPersons("Mattis Mueller").personList;


        List<Person> expectedResult = new()
        {
            new() {FirstName = "Mattis", LastName = "Mueller"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void MultiblePersons()
    {
        List<Person> allPersons = new()
        {
            new() {FirstName = "Mattis", LastName = "Mueller"},
            new() {FirstName = "Tizian", LastName = "Tischer"},
            new() {FirstName = "Cristian", LastName = "Cramer"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis Mueller")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian Tischer")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian Cramer")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);
        var result = stme.GetPersons("Mattis Mueller,tizian Tischer").personList;

        List<Person> expectedResult = new()
        {
            new() {FirstName = "Mattis", LastName = "Mueller"},
            new() {FirstName = "Tizian", LastName = "Tischer"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void NoValidPerson()
    {
        var allPersons = new List<Person>()
        {
            new() {FirstName = "Mattis", LastName = "Mueller"},
            new() {FirstName = "Tizian", LastName = "Tischer"},
            new() {FirstName = "Cristian", LastName = "Cramer"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis Mueller")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian Tischer")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian Cramer")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);
        var result = stme.GetPersons("Tizian Tischer, Mattis Benjamin Mueller");


        List<Person> expectedPersonListResult = new()
        {
            new() {FirstName = "Mattis Benjamin", LastName = "Mueller", DateOfBirth = DateTime.Parse("01/01/1900")},
        };

        result.personList.Should().BeEquivalentTo(expectedPersonListResult);
        result.errorHasOccured.Should().BeTrue();
    }
}
