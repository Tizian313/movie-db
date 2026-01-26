using YourMovieDB.OperationContracts;
using YourMovieDB.OperationContracts.Interfaces;
using YourMovieDB.Models;
using Database.Repository;
using Database.Repository.Interfaces;
using MovieDB.SharedModels;
using Moq;

namespace YourMovieDB.Test
{
    [TestClass]
    public class MovieDBUnitTest
    {
        private IDraw? Draw;
        private IDataChanger? DataChanger;
        private IMovieRepository? Repository;

        [TestMethod]
        public void TestDeleteMovie()
        {
            //Create mock
            var deleteMock = new Mock<IMovieRepository>();
            deleteMock.Setup((m => m.Remove())).Returns();

            // Input data
            List<Person> persons = [ new ("Frank", "Darabont", DateTime.Parse("2010-01-01T00:00:00") ),
                                     new ("Tim", "Robbins", DateTime.Parse("2003-01-01T00:00:00") )];

            List<Movie> movies = [ new ("The Godfather", [Genres.Drama], [0], [1], 6.9f),
                                   new ("Bad Movie", [Genres.Documentary], [1], [0], 0.1f)];
            // Input data


            // Input movie
            Movie movieToDelete = movies[1];
            // Input movie

            var repository = new Repository();
            var dataChanger = new DataChanger(Draw!, Repository!);

            repository.Remove(movieToDelete);




            Assert.AreEqual(1, mock..Count);
            Assert.AreEqual("The Godfather", data.AllMovies[0].Name);
            Assert.AreEqual(Genres.Drama, data.AllMovies[0].Genres[0]);
        }


        [TestMethod]
        public void TestDeletePerson()
        {
            // Input data
            List<Person> persons = [ new ("Frank", "Darabont", DateTime.Parse("2010-01-01T00:00:00") ),
                                     new ("Tim", "Robbins", DateTime.Parse("2003-01-01T00:00:00") )];

            List<Movie> movies = [ new ("The Godfather", [Genres.Drama], [0], [1], 6.9f),
                                   new ("Bad Movie", [Genres.Documentary], [1], [0], 0.1f)];

             = new(persons, movies);
            // Input data


            // Input person
            Person personToDelete = persons[1];
            // Input person


            var dataChanger = new DataChanger(Draw!, Repository!);

            dataChanger.DeletePerson(personToDelete, data, true);


            Assert.AreEqual(1, data.AllPersons.Count);
            Assert.AreEqual("Frank", data.AllPersons[0].FirstName);
            Assert.AreEqual(DateTime.Parse("2010-01-01T00:00:00"), data.AllPersons[0].DateOfBirth);
        }



        [TestMethod]
        public void IDtoPersonName()
        {
            // Input data
            List<Person> persons = [ new ("Frank", "Darabont", DateTime.Parse("2010-01-01T00:00:00") ),
                                     new ("Tim", "Robbins", DateTime.Parse("2003-01-01T00:00:00")    )];

            // Input data


            var dataChanger = new DataChanger(Draw!, Repository!);

            List<int> input = [0]; // Test with one person
            string output = dataChanger.IDtoPersonNames(input, data);
            Assert.AreEqual("Frank Darabont", output);

            input = [1, 0]; // Test with multiple persons
            output = dataChanger.IDtoPersonNames(input, data);
            Assert.AreEqual("Tim Robbins, Frank Darabont", output);
        }
    }
}