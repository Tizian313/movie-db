using Database.Repository.Interfaces;
using MovieDB.SharedModels;
using System.Globalization;
using System.Text.Json;
using YourMovieDB.Models;
using YourMovieDB.OperationContracts.Interfaces;

namespace YourMovieDB.OperationContracts;


public class DataChanger : IDataChanger
{
    private IDraw Draw;
    private IBaseRepository<Person> personRepository;
    private IBaseRepository<Movie> movieRepository;

    public DataChanger(IDraw draw, IBaseRepository<Person> personRepository, IBaseRepository<Movie> movieRepository)
    {
        this.Draw = draw;
        this.personRepository = personRepository;
        this.movieRepository = movieRepository;
    }

    public Person? CreatePerson(string invalidName)
    {
        int selection = 0;
        Person person = new("", "", DateTime.Parse("01.01.2000"), 0); // Creator ID is set by API-Controller

        List<Selection> loadedMenu = [
            new("", null),
            new("", null),
            new("", null),
            new("Cancel", null),
            new("Save and continue", null)];

        while (true)
        {
            List<string> menuText = [$"First name: {person.FirstName}",
                                     $"Last name: {person.LastName}",
                                     $"Date of birth: {person.DateOfBirth:(dd.MM.yyyy)}"];

            for (int i = 0; i < menuText.Count; i++)
            {
                loadedMenu[i].Name = menuText[i];
            }

            Console.Clear();
            Draw.Logo();
            Draw.InfoBox($"\"{invalidName}\" WAS NOT FOUND!");
            Draw.InfoBox("WHAT ARE THE INFOS FOR THE NEW ACTOR/DIRECTOR!");

            Draw.Box(loadedMenu, selection);

            ConsoleKey input = Console.ReadKey(true).Key;

            if (Globals.UpButtons.Contains(input))
            {
                if (selection == 0)
                {
                    selection = loadedMenu.Count - 1;
                }
                else
                {
                    selection--;
                }
            }

            if (Globals.DownButtons.Contains(input))
            {
                if (selection == loadedMenu.Count - 1)
                {
                    selection = 0;
                }
                else
                {
                    selection++;
                }
            }

            if (Globals.SelectButtons.Contains(input))
            {
                switch (selection)
                {
                    case < 3:
                        person = CreatePersonEdit(selection, person);
                        break;

                    case 3:
                        return null;

                    case 4:
                        if (person.FirstName == "" || person.LastName == "")
                        {
                            return null;
                        }
                        return person;
                }
            }

            if (Globals.BackButtons.Contains(input))
            {
                return null;
            }
        }
    }

    public Person CreatePersonEdit(int selection, Person person)
    {
        List<string> questions = ["WHAT'S THE FIRST NAME?",
                                  "WHAT'S THE LAST NAME?",
                                  "WHEN WAS THE PERSON BORN?"];

        Console.Clear();
        Draw.InfoBox(questions[selection]);

        Console.WriteLine();
        Draw.LeftSpace(30);
        Draw.ColorWrite("> ", ConsoleColor.White, lineBreak: false);

        string? input;
        input = Console.ReadLine();
        if (input == null)
            input = "";
        
        if (selection == 0) //Case for first name input
        {
            person.FirstName = input.Trim();
        }

        if (selection == 1) //Case for last name input
        {
            person.LastName = input.Trim();
        }

        if (selection == 2) //Date of birth input
        {
            try
            {
                person.DateOfBirth = DateTime.Parse(input);
            }
            catch { }
        }

        return person;
    }

    public void CreateMovie()
    {
        var movie = new Movie("", [], [], [], 0.0f, 0); // Creator ID is set by API-Controller
        var createdMovie = EditMovieCore(movie);

        if (createdMovie != null)
            movieRepository.Add(movie);
    }

    public void EditMovie(Movie movie)
    {
        var serialized = JsonSerializer.Serialize(movie);
        var movieCopy = JsonSerializer.Deserialize<Movie>(serialized);

        if (movieCopy == null)
            throw new Exception("Deserialization error, which should be impossible.");

        Movie? editedMovie = EditMovieCore(movieCopy);
        if (editedMovie != null)
        {
            movieRepository.Remove(movie.Id);
            movieRepository.Add(editedMovie);
        }
        return;
    }

    private Movie? EditMovieCore(Movie movie)
    {
        int selection = 0;

        List<Selection> loadedMenu = [
            new("", null),
            new("", null),
            new("", null),
            new("", null),
            new("", null),
            new("Cancel and back", null),
            new("Save and back", null)];

        while (true)
        {
            List<string> menuText = ["Name: " + movie.Name,
                                     "Genres: " + string.Join(", ", movie.Genres.ToArray()),
                                     "Director: " + IDtoPersonNames(movie.Director_IDs),
                                     "Stars: " + IDtoPersonNames(movie.Star_IDs),
                                     "Rating: " + movie.Rating.ToString("0.0")];

            for (int i = 0; i < menuText.Count; i++)
            {
                loadedMenu[i].Name = menuText[i];
            }

            Console.Clear();
            Draw.Logo();
            Draw.InfoBox("SELECT WHAT YOU WANT TO EDIT!");

            Draw.Box(loadedMenu, selection);

            ConsoleKey input = Console.ReadKey(true).Key;

            if (Globals.UpButtons.Contains(input))
            {
                if (selection == 0)
                {
                    selection = loadedMenu.Count - 1;
                }
                else
                {
                    selection--;
                }
            }

            if (Globals.DownButtons.Contains(input))
            {
                if (selection == loadedMenu.Count - 1)
                {
                    selection = 0;
                }
                else
                {
                    selection++;
                }

            }

            if (Globals.SelectButtons.Contains(input))
            {
                switch (selection)
                {
                    case < 5:
                        movie = CreateMovieEdit(selection, movie);
                        break;

                    case 5:
                        return null;

                    case 6:
                        return movie;
                }
            }

            if (Globals.BackButtons.Contains(input))
            {
                return null;
            }
        }
    }


    public string IDtoPersonNames(List<int> input)
    {
        string answerString = "";

        foreach (var person_id in input)
        {
            Person? foundPerson = personRepository.GetAll().FirstOrDefault(q => q.Id == person_id);

            if (foundPerson == null)
                answerString += "Deleted person, ";
            else
                answerString += foundPerson!.FirstName + " " + foundPerson.LastName + ", ";
        }

        if (answerString == "")
            return "";

        return answerString.Substring(0, answerString.Length - 2);
    }

    public List<int> InputPersons(string input)
    {
        List<string> nameList = [.. input.Split(',')]; //Splits input and makes it lowercase
        List<Person> personsFound = [];

        foreach (var name in nameList)
        {
            bool foundSomeone = false;
            string trimmedName = name.Trim(' ');

            foreach (var person in personRepository.GetAll())
            {
                string globalPersonsName = (person.FirstName + " " + person.LastName).ToLower();
                if (globalPersonsName == trimmedName.ToLower())
                {
                    personsFound.Add(person);
                    foundSomeone = true;
                    break;
                }
            }

            if (!foundSomeone)
            {
                Person? newPerson = CreatePerson(trimmedName);
                if (newPerson != null)
                {
                    personsFound.Add(newPerson);
                    personRepository.Add(newPerson);
                }
            }
        }

        List<int> personsToReturn = []; // Converts Person list to list of IDs and remove duplicates
        foreach (var person in personsFound)
        {
            if (personsToReturn.Contains(person.Id) == false)
            {
                personsToReturn.Add(person.Id);
            }
        }
        return personsToReturn;


    }

    public Movie CreateMovieEdit(int selection, Movie movie)
    {
        List<string> questions = ["WHAT'S THE NAME OF THE MOVIE?",
                                  "WHAT'S ARE THE GENRES OF THE MOVIE?",
                                  "WHO'S THE DIRECTOR IN THIS MOVIE?",
                                  "WHO ARE THE STARS IN THIS MOVIE?",
                                  "WHAT'S THE THE Rating OF THE MOVIE?"];

        Console.Clear();
        Draw.InfoBox(questions[selection]);

        if (selection == 1)
        {
            Draw.InfoBox("OPTIONS: Action, Adventure, Animation, Anime, Comedy, Crime, Documentary, Drama, Family, Fantasy");
            Draw.InfoBox("Horror, Lifestyle, Musical, Mystery, Romance, SciFi, Seasonal, Short, Sport, Thriller, Western");
        }

        Console.WriteLine();
        Draw.LeftSpace(30);
        Draw.ColorWrite("> ", ConsoleColor.White, lineBreak: false);
        string? input = Console.ReadLine();

        if (input == null)
            input = "";

        if (selection == 0) //Case for name input
        {
            movie.Name = input.Trim();
        }

        if (selection == 1) //Case for genre input
        {
            movie.Genres = [];
            Genres validEnum;

            List<string> inputs = input.Split(',').ToList<string>();

            foreach (var splitInput in inputs)
            {
                string properCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(splitInput.ToLower());
                if (Enum.TryParse(properCase, out validEnum)) // Adds all readable Genres to movie.Genres
                {
                    if (movie.Genres.Contains(validEnum) == false)
                    {
                        movie.Genres.Add(validEnum);
                    }
                }
            }
        }

        if (selection == 2) //Case for director input
        {
            movie.Director_IDs = InputPersons(input);
        }

        if (selection == 3) //Case for star input
        {
            movie.Star_IDs = InputPersons(input);
        }

        if (selection == 4) //Case for Rating input
        {
            movie.Rating = 0.0f;
            try
            {
                movie.Rating = float.Parse(input);
            }
            catch { }

            if (movie.Rating > 10.0f)
            {
                movie.Rating = 10.0f;
            }

            if (movie.Rating < 0.0f)
            {
                movie.Rating = 0.0f;
            }
        }
        return movie;
    }
}
