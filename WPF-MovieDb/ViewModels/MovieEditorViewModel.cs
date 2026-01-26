using CommunityToolkit.Mvvm.ComponentModel;
using MovieDB.SharedModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPF_MovieDb.Models;
using WPF_MovieDb.Models.Interfaces;
using WPF_MovieDb.Models.Items;
using WPF_MovieDb.Services.Interfaces;
using WPF_MovieDb.Services.Models;

namespace WPF_MovieDb.ViewModels;


public partial class MovieEditorViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly IStringsToMovieElements stringsToMovieElements;
    private readonly IPersonAPIService personAPIService;
    private readonly IMovieAPIService movieAPIService;
    private readonly IGenerateInitialText generateInitialText;
    private readonly INavigationService navigationService;

    public MovieEditorViewModel(IPersonAPIService personAPIService, IMovieAPIService movieAPIService, INavigationService navigationService, IGenerateInitialText generateInitialText, IStringsToMovieElements stringsToMovieElements)
    {
        this.AddGenre = new DelegateCommand((_) => GenresInput = OnAddCommand(SelectedGenre, GenresInput));
        this.AddActor = new DelegateCommand((_) => ActorInput = OnAddCommand(SelectedActor, ActorInput));
        this.AddDirector = new DelegateCommand((_) => DirectorInput = OnAddCommand(SelectedDirector, DirectorInput));

        this.Enter = new DelegateCommand((_) => OnEnter());
        this.Cancel = new DelegateCommand((_) => GoBack());
        
        this.stringsToMovieElements = stringsToMovieElements;
        this.movieAPIService = movieAPIService;
        this.personAPIService = personAPIService;
        this.generateInitialText = generateInitialText;
        this.navigationService = navigationService;
    }

    public ICommand AddGenre { get; set; }
    public ICommand AddActor { get; set; }
    public ICommand AddDirector { get; set; }
    public ICommand Enter { get; set; }
    public ICommand Cancel { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    [ObservableProperty]
    private Movie movie;

    [ObservableProperty]
    private string loadedFrom;

    [ObservableProperty]
    private bool fromAddPerson;

    string nameInput;
    public string NameInput
    {
        get => nameInput;
        set
        {
            nameInput = value;
            this.RaisePropertyChanged();
        }
    }

    string genresInput;
    public string GenresInput
    {
        get => genresInput;
        set
        {
            genresInput = value;
            this.RaisePropertyChanged();
        }
    }

    string directorInput;
    public string DirectorInput
    {
        get => directorInput;
        set
        {
            directorInput = value;
            this.RaisePropertyChanged();
        }
    }

    string actorInput;
    public string ActorInput
    {
        get => actorInput;
        set
        {
            actorInput = value;
            this.RaisePropertyChanged();
        }
    }

    string starWidth;
    public string StarWidth 
    { 
        get => starWidth; 
        set 
        {
            starWidth = value;
            this.RaisePropertyChanged();
        } 
    }

    string sliderValue;
    public string SliderValue
    {
        get => sliderValue;
        set 
        {
            sliderValue = value;
            StarWidth = (Math.Round( 4 * float.Parse(value) )).ToString();
            RatingText = (float.Parse(value) / 10).ToString("0.0");
            this.RaisePropertyChanged();
        }
    }

    string ratingText;
    public string RatingText 
    {
        get => ratingText;
        set
        {
            ratingText = value;
            this.RaisePropertyChanged();
        }
    }


    public string TitleText
    {
        get
        {
            if (LoadedFrom == "MenuViewModel")
                return "Add movie";
            else
                return "Edit movie";
        }
    }

    public object SelectedGenre { get; set; }
    public object SelectedActor { get; set; }
    public object SelectedDirector { get; set; }


    private ObservableCollection<CanvasLabel> genreItemSource;
    public ObservableCollection<CanvasLabel> GenreItemsSource
    {
        get
        {
            genreItemSource = BuildGenreItemSource();
            return genreItemSource;
        }
        set { genreItemSource = value; }
    }


    private ObservableCollection<CanvasLabel> personItemsSource;
    public ObservableCollection<CanvasLabel> PersonItemsSource
    {
        get
        {
            personItemsSource = BuildPersonItemSource();
            return personItemsSource;
        }
        set { personItemsSource = value; }
    }


    ObservableCollection<CanvasLabel> BuildGenreItemSource()
    {
        ObservableCollection<CanvasLabel> source = new();
        List<Genres> allGenres = Enum.GetValues(typeof(Genres)).OfType<Genres>().ToList();

        foreach (Genres genre in allGenres)
            source.Add(new() { CanvasText = genre.ToString() });

        return source;
    }

    ObservableCollection<CanvasLabel> BuildPersonItemSource()
    {
        ObservableCollection<CanvasLabel> source = new();

        foreach (Person person in personAPIService.GetAll())
            source.Add(new() { CanvasText = $"{person.FirstName} {person.LastName}" });

        return source;
    }


    public string OnChangedToView
    {
        get
        {
            if (FromAddPerson) // If the AddPerson view was open, the variables are not overwritten.
                return string.Empty;

            switch (LoadedFrom)
            { 
                case "ShowMovieDetailedViewModel": // User tries to edit movie
                    NameInput = Movie.Name;
                    GenresInput = generateInitialText.ForGenres(Movie);
                    DirectorInput = generateInitialText.ForDirector(Movie);
                    ActorInput = generateInitialText.ForActor(Movie);
                    SliderValue = generateInitialText.ForRating(Movie);
                    break;

                case "MenuViewModel": // User tries to add movie
                    NameInput = "";
                    GenresInput = "";
                    DirectorInput = "";
                    ActorInput = "";
                    SliderValue = "0";
                    break;

                default:
                    throw new Exception($"The value for \"LoadedFrom\" is invalid.\n{LoadedFrom} <--- value");
            }
            return string.Empty;
        }
    }


    string OnAddCommand(object? selectedObject, string inputField)
    {
        if (selectedObject == null)
            return inputField;

        var text = selectedObject.GetType().GetProperties()[0].GetValue(selectedObject);

        string separator = ", ";

        if (inputField.Length == 0)
            separator = "";

        return inputField + separator + text;
    }

    void OnEnter()
    {
        var name = stringsToMovieElements.GetName(NameInput);
        var genres = stringsToMovieElements.GetGenres(GenresInput);
        var rating = stringsToMovieElements.GetRating(SliderValue);

        List<Person> errorPersons = new();
        string errorMessage = name.errorMessage + genres.errorMessage + rating.errorMessage;


        var directors = stringsToMovieElements.GetPersons(DirectorInput);
        var actors = stringsToMovieElements.GetPersons(ActorInput);

        if (directors.errorHasOccured || actors.errorHasOccured) // Sets error message and puts all invalid persons in errorPersons.
        {
            if (errorMessage != string.Empty)
                errorMessage += "\n";

            errorMessage += "One or more persons couldn't be recognized.\n"
                            + "You need to create a new person or remove the unrecognized name from your input.\n";

            if (directors.errorHasOccured)
                errorPersons.AddRange(directors.personList);

            if (actors.errorHasOccured)
                errorPersons.AddRange(actors.personList);
        }

        if (errorMessage == string.Empty)
            SaveMovie(name.name, genres.genres, rating.rating, directors.personList, actors.personList);
        else
            HandelInputError(errorMessage, errorPersons);
    }

    void SaveMovie(string name, List<Genres> genres, float rating, List<Person> directors, List<Person> actors)
    {
        // Person lists to list of persons ids.
        List<int> actorIds = new();
        List<int> directorIds = new();

        foreach (var d in directors)
            directorIds.Add(d.Id);

        foreach (var a in actors)
            actorIds.Add(a.Id);

        // Replaces or adds the movie in the database.
        if (LoadedFrom == "ShowMovieDetailedViewModel")
            movieAPIService.Remove(Movie.Id);

        movieAPIService.Add(new Movie()
        {
            Name = name,
            Genres = genres,
            Director_IDs = directorIds,
            Star_IDs = actorIds,
            Rating = rating,
            CreatorId = 0
        });

        GoBack();
    }

    void GoBack()
    {
        if (LoadedFrom == "MenuViewModel")
            navigationService.Navigate<MenuViewModel>();

        else
            navigationService.Navigate<MovieSearchViewModel>();
    }

    void HandelInputError(string errorMessage, List<Person> persons)
    {
        MessageBox.Show(errorMessage, "Input error.");

        if (persons.Count > 0)
            navigationService.Navigate<AddPersonViewModel>(x =>
            {
                x.PersonsLeft = persons;
            });
    }

    private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (!string.IsNullOrEmpty(propertyName))
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
