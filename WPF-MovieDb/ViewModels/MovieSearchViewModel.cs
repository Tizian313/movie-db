using CommunityToolkit.Mvvm.ComponentModel;
using MovieDB.SharedModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPF_MovieDb.Models;
using WPF_MovieDb.Models.Interfaces;
using WPF_MovieDb.Models.Items;
using WPF_MovieDb.Services.Interfaces;
using WPF_MovieDb.Services.Models;

namespace WPF_MovieDb.ViewModels;


public partial class MovieSearchViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly INavigationService navigationService;
    private readonly IMovieAPIService movieAPIService;
    private readonly IMovieSearch movieSearch;

    public MovieSearchViewModel(INavigationService navigationService, IMovieAPIService movieAPIService, IMovieSearch movieSearch)
    {
        this.ChangeOrder = new DelegateCommand((_) => invertedList = !invertedList);
        this.ChangeAllBoxesCommand = new DelegateCommand((parameter) => ButtonGenreSelection(selectAll: bool.Parse((string)parameter)));
        this.Return = new DelegateCommand((_) => navigationService.Navigate<MenuViewModel>());

        this.movieSearch = movieSearch;
        this.movieAPIService = movieAPIService;
        this.navigationService = navigationService;
    }

    public ICommand ChangeOrder { get; set; }
    public ICommand ChangeAllBoxesCommand { get; set; }
    public ICommand Return { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    string heightOfCanvas;
    public string HeightOfCanvas
    {
        get => heightOfCanvas;
        set
        {
            heightOfCanvas = value;
            this.RaisePropertyChanged();
        }
    }

    private ObservableCollection<CanvasButton> movieItemSource;
    public ObservableCollection<CanvasButton> MovieItemSource
    {
        get => movieItemSource;
        set
        {
            movieItemSource = value;
            this.RaisePropertyChanged();
        }
    }

    private ObservableCollection<LabeledCheckBox> genreSelectionSource;
    public ObservableCollection<LabeledCheckBox> GenreSelectionSource
    {
        get => genreSelectionSource;
        set
        {
            if (value != genreSelectionSource)
            {
                genreSelectionSource = value;
                this.RaisePropertyChanged();
            }
        }
    }


    string searchBarInput;
    public string SearchBarInput
    {
        get => searchBarInput;
        set
        {
            if (value != searchBarInput)
            {
                searchBarInput = value;

                UpdateMovieItemSource();
                this.RaisePropertyChanged();
            }
        }
    }

    CanvasLabel selectedSearchBy;
    public CanvasLabel SelectedSearchBy
    {
        get => selectedSearchBy;
        set
        {
            selectedSearchBy = value;
            UpdateMovieItemSource();
            this.RaisePropertyChanged();
        }
    }

    CanvasLabel selectedRankBy;
    public CanvasLabel SelectedRankBy
    {
        get => selectedRankBy;
        set
        {
            selectedRankBy = value;
            UpdateMovieItemSource();
            this.RaisePropertyChanged();
        }
    }


    public ObservableCollection<CanvasLabel> SearchBySource
    {
        get
        {
            ObservableCollection<CanvasLabel> source = new()
            {
                new() {CanvasText = "movie name" },
                new() {CanvasText = "persons contributed" }
                //new() {CanvasText = "genres" }
            };

            return source;
        }
    }

    public ObservableCollection<CanvasLabel> RankBySource
    {
        get
        {
            ObservableCollection<CanvasLabel> source = new()
            {
                new() {CanvasText = "alphabetically" },
                new() {CanvasText = "by rating" }
            };

            return source;
        }
    }

    bool _invertedList;
    bool invertedList
    {
        get => _invertedList;
        set
        {
            _invertedList = value;
            UpdateMovieItemSource();
        }
    }

    public string OnChangedToView
    {
        get
        {
            // Sets default selections
            _invertedList = false;

            GenreSelectionSource = BuildGenreSelectionSource(allSelected: false);

            selectedSearchBy = new() { CanvasText = "movie name" };
            selectedRankBy = new() { CanvasText = "alphabetically" };

            SearchBarInput = "";

            UpdateMovieItemSource();

            return string.Empty;
        }
    }

    void UpdateMovieItemSource()
    {
        List<Movie> movieList = movieSearch.GetMovieList(searchBarInput, SelectedSearchBy.CanvasText, SelectedRankBy.CanvasText, invertedList, GenreSelectionSource);

        // BuildItemSource
        ObservableCollection<CanvasButton> source = new();

        foreach (Movie movie in movieList)
            source.Add(new()
            {
                StarWidth = (int)Math.Round(250 * (movie.Rating / 10)),
                CanvasCommand = new DelegateCommand((parameter) => OnCanvasButtonClick((int)parameter)),
                CanvasButtonText = movie.Name,
                CanvasButtonParameter = movie.Id
            });

        MovieItemSource = source;

        // Update height of canvas
        HeightOfCanvas = (MovieItemSource.Count * 80).ToString(); // Times 80 because canvas button height is 80px
    }

    ObservableCollection<LabeledCheckBox> BuildGenreSelectionSource(bool allSelected) // allSelected makes all boxes checked or unchecked
    {
        ObservableCollection<LabeledCheckBox> source = new();
        List<Genres> allGenres = Enum.GetValues(typeof(Genres)).OfType<Genres>().ToList();

        // Builds all items for GenreSelection.
        foreach (Genres genre in allGenres)
            source.Add(new()
            {
                OnToggleCommand = new DelegateCommand((_) => UpdateMovieItemSource()),
                Text = genre.ToString(),
                IsChecked = allSelected
            });

        return source;
    }

    void OnCanvasButtonClick(int id)
    {
        Movie? movie = movieAPIService.Get(id)
        ?? throw new Exception("The ID passed to OnCanvasButtonClick didn't match any movie.");

        navigationService.Navigate<ShowMovieDetailedViewModel>
            (x => { x.LoadedMovie = movie; });
    }

    void ButtonGenreSelection(bool selectAll)
    {
        GenreSelectionSource = BuildGenreSelectionSource(selectAll);
        UpdateMovieItemSource();
    }

    private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (!string.IsNullOrEmpty(propertyName))
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
