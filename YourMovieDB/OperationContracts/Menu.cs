using Database.Repository.Interfaces;
using MovieDB.SharedModels;
using YourMovieDB.Models;
using YourMovieDB.OperationContracts.Interfaces;

namespace YourMovieDB.OperationContracts
{
    public class Menu : IMenu
    {
        private IDraw Draw;
        private IDataChanger DataChanger;
        private IPersonRepository personRepository;
        private IMovieRepository movieRepository;

        public Menu(IDraw draw, IDataChanger dataChanger, IPersonRepository personRepository, IMovieRepository movieRepository)
        {
            this.Draw = draw;
            this.DataChanger = dataChanger;
            this.personRepository = personRepository;
            this.movieRepository = movieRepository;
        }

        private void Create(List<Selection> loadedMenu, List<string> infoText, bool returnAfterLamda = false)
        {
            int selection = 0;
            
            while (true)
            {
                Console.Clear();
                Draw.Logo();
                
                foreach (var line in infoText)
                {
                    Draw.InfoBox(line);
                }

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
                    if (loadedMenu[selection].Redirect == null)
                        return;
                    else
                        loadedMenu[selection].Redirect!();

                    if (returnAfterLamda)
                    {
                        return;
                    }
                }

                if (Globals.BackButtons.Contains(input))
                {
                    return;
                }
            }
        }

        public void MainMenu()
        {
            List<Selection> mainMenu = [new Selection("Search movies", () => SearchMovies()) ,
                                        new Selection("Add movie", () => DataChanger.CreateMovie()),
                                        new Selection("Delete actor or director", () => DeletePersonMenu()),
                                        new Selection("Exit", () => Environment.Exit(0))];

            Create(mainMenu, []);
            Environment.Exit(0);
        }

        public void ListMoviesRanked(RankingMethods method)
        {
            List<Selection> rankedMovieList = [];
            List<Movie> sortedMovieList;

            switch (method)
            {
                case RankingMethods.AlphabeticalBeginningFromA:
                    sortedMovieList = movieRepository.GetAll().OrderBy(o => o.Name).ToList();
                    break;
                case RankingMethods.AlphabeticalBeginningFromZ:
                    sortedMovieList = movieRepository.GetAll().OrderByDescending(o => o.Name).ToList();
                    break;

                case RankingMethods.RatingBeginningFromLowest:
                    sortedMovieList = movieRepository.GetAll().OrderBy(o => o.Rating).ToList();
                    break;

                case RankingMethods.RatingBeginningFromHighest:
                    sortedMovieList = movieRepository.GetAll().OrderByDescending(o => o.Rating).ToList();
                    break;

                default:
                    throw new Exception($"ListMoviesRanked didn't implement the RankingMethod {method}");
            }

            foreach (var movie in sortedMovieList)
            {
                rankedMovieList.Add(new(movie.Name, () => ShowMovie(movie)));
            }
            rankedMovieList.Add(new("BACK", null));

            Create(rankedMovieList, ["SELECT YOUR SEARCHED MOVIE!"], true);
        }

        public void SearchMovies()
        {
            List<Selection> searchMenu = [new Selection("Search by name", () => SearchByString(SearchMethod.ByMovieName)),
                                          new Selection("Search by persons", () => SearchByString(SearchMethod.ByPersonName)),
                                          new Selection("Search by genre", () => SearchByGenre()),
                                          new Selection("Ranked alphabetically (A-Z)", () => ListMoviesRanked(RankingMethods.AlphabeticalBeginningFromA)),
                                          new Selection("Ranked alphabetically (Z-A)", () => ListMoviesRanked(RankingMethods.AlphabeticalBeginningFromZ)),
                                          new Selection("Ranked by Rating (ascending)", () => ListMoviesRanked(RankingMethods.RatingBeginningFromHighest)),
                                          new Selection("Ranked by Rating (descending)", () => ListMoviesRanked(RankingMethods.RatingBeginningFromLowest)),
                                          new Selection("BACK", null)];

            Create(searchMenu, ["WHAT DO YOU WANT TO SEARCH BY?"]);
        }

        public void SearchByString(SearchMethod method)
        {
            List<Selection> moviesFound = [];

            Console.Clear();
            Draw.Logo();

            Draw.InfoBox("WRITE WHAT CHARACTER STRING TO SEARCH FOR!");

            Console.WriteLine();
            Draw.LeftSpace(44);
            Console.Write("> ");

            string? input = Console.ReadLine();

            if (input != null)
                input = input.ToLower();
            else
                input = "";


                if (method == SearchMethod.ByMovieName) //If movies are searched by their name.
            {
                foreach (var movie in movieRepository.GetAll())
                {
                    if (movie.Name.ToLower().Contains(input))
                    {
                        moviesFound.Add(new(movie.Name, () => ShowMovie(movie)));
                    }
                }
            }

            if (method == SearchMethod.ByPersonName) // If movies are searched by persons that contributed to it.
            {
                List<Movie> foundMovies = [];

                foreach (var movie in movieRepository.GetAll())
                {
                    List<int> ids = [];
                    ids.AddRange(movie.Star_IDs);
                    ids.AddRange(movie.Director_IDs); // ids = all ids of persons in the movie.

                    foreach (var person in personRepository.GetAll())
                    {
                        if (ids.Contains(person.Id)
                        && (person.FirstName + " " + person.LastName).ToLower().Contains(input) 
                        && !foundMovies.Contains(movie))
                        {
                            foundMovies.Add(movie);
                            moviesFound.Add(new(movie.Name, () => ShowMovie(movie)));
                        }
                    }
                }
            }

            if (moviesFound.Count == 0) // if no matching movie was found.
            {
                return;
            }

            moviesFound.Add(new("BACK", null));
            Create(moviesFound, ["SELECT YOUR SEARCHED MOVIE!"], true);
        }


        public void ShowMovie(Movie movie)
        {
            List<Selection> showMovieMenu = [new("Edit movie", () => DataChanger.EditMovie(movie)),
                                             new("Delete movie", () => movieRepository.Remove(movie.Id)),
                                             new("BACK", null)];

            List<string> infoText = ["Name: " + movie.Name,
                                     "Genres: " + string.Join(", ", movie.Genres),
                                     "Director: " + DataChanger.IDtoPersonNames(movie.Director_IDs),
                                     "Stars: " + DataChanger.IDtoPersonNames(movie.Star_IDs),
                                     "Rating: " + movie.Rating];

            Create(showMovieMenu, infoText, true);
        }

        public void ListByGenre(Genres genre)
        {
            List<Selection> foundMovies = [];

            foreach (Movie movie in movieRepository.GetAll())
            {
                if (movie.Genres.Contains(genre))
                {
                    foundMovies.Add(new(movie.Name, () => ShowMovie(movie)));
                }
            }

            if (foundMovies.Count != 0)
            {
                foundMovies.Add(new("BACK", null));
                Create(foundMovies, [$"MOVIES WITH GENRE \"{genre.ToString().ToUpper()}\":"], true);
            }
            return;
        }

        public void SearchByGenre()
        {
            List<Selection> genreSearchMenu = [];

            foreach (Genres genre in Enum.GetValues(typeof(Genres)))
            {
                genreSearchMenu.Add(new(genre.ToString(), () => ListByGenre(genre)));
            }

            Create(genreSearchMenu, ["SELECT WHAT GENRE YOU WANT TO SEE!"], true);
        }

        public void DeletePersonMenu()
        {
            List<Selection> selectablePersons = [];

            foreach (var person in personRepository.GetAll())
            {
                string text = $"{person.FirstName} {person.LastName} | {person.DateOfBirth:dd.MM.yyyy}";
                selectablePersons.Add(new(text, () => personRepository.Remove(person.Id)));
            }

            selectablePersons.Add(new("BACK", null));
            Create(selectablePersons, ["SELECT A PERSON TO DELETE!"], true);
        }
    }
}
