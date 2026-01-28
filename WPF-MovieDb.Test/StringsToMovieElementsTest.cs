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
            new() {FirstName = "Mattis", LastName = "lm0"},
            new() {FirstName = "Tizian", LastName = "lm1"},
            new() {FirstName = "Cristian", LastName = "lm2"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis lm0")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian lm1")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian lm2")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);

        var result = stme.GetPersons("Mattis lm0").personList;


        List<Person> expectedResult = new()
        {
            new() {FirstName = "Mattis", LastName = "lm0"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void MultiblePersons()
    {
        List<Person> allPersons = new()
        {
            new() {FirstName = "Mattis", LastName = "lm0"},
            new() {FirstName = "Tizian", LastName = "lm1"},
            new() {FirstName = "Cristian", LastName = "lm2"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis lm0")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian lm1")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian lm2")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);
        var result = stme.GetPersons("Mattis lm0,tizian lm1").personList;

        List<Person> expectedResult = new()
        {
            new() {FirstName = "Mattis", LastName = "lm0"},
            new() {FirstName = "Tizian", LastName = "lm1"}
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void NoValidPerson()
    {
        var allPersons = new List<Person>()
        {
            new() {FirstName = "Mattis", LastName = "lm0"},
            new() {FirstName = "Tizian", LastName = "lm1"},
            new() {FirstName = "Cristian", LastName = "lm2"}
        };

        Mock<IPersonAPIService> personRepoMock = new();
        personRepoMock.Setup(x => x.Get("mattis lm0")).Returns(allPersons[0]);
        personRepoMock.Setup(x => x.Get("tizian lm1")).Returns(allPersons[1]);
        personRepoMock.Setup(x => x.Get("cristian lm2")).Returns(allPersons[2]);

        var stme = new StringsToMovieElements(personRepoMock.Object);
        var result = stme.GetPersons("Tizian lm1, Mattis Benjamin lm0");


        List<Person> expectedPersonListResult = new()
        {
            new() {FirstName = "Mattis Benjamin", LastName = "lm0", DateOfBirth = DateTime.Parse("01/01/1900")},
        };

        result.personList.Should().BeEquivalentTo(expectedPersonListResult);
        result.errorHasOccured.Should().BeTrue();
    }
}
