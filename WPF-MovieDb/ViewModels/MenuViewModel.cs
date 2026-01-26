using CommunityToolkit.Mvvm.ComponentModel;
using MovieDB.SharedModels;
using System.Windows.Input;
using WPF_MovieDb.Models;
using WPF_MovieDb.Services.Models;

namespace WPF_MovieDb.ViewModels;


public partial class MenuViewModel : ObservableObject
{
    private readonly INavigationService navigationService;

    public MenuViewModel(INavigationService navigationService)
    {
        this.ToSearchMoviesCommand = new DelegateCommand((_) => navigationService.Navigate<MovieSearchViewModel>());
        this.ToAddMovieCommand = new DelegateCommand((_) => OnAddMovie());
        this.Logout = new DelegateCommand((_) => navigationService.Navigate<MainViewModel>());

        this.navigationService = navigationService;
    }

    public ICommand ToSearchMoviesCommand { get; set; }
    public ICommand ToAddMovieCommand { get; set; }
    public ICommand Logout { get; set; }


    string squish;
    public string Squish
    {
        get => squish;
        set
        {
            squish = value;
        }
    }

    void OnAddMovie()
    {
        Movie movieTemplate = new Movie()
        {
            Name = "",
            Genres = new(),
            Director_IDs = new(),
            Star_IDs = new(),
            Rating = 0.0f
        };

        navigationService.Navigate<MovieEditorViewModel>(x =>
        {
            x.Movie = movieTemplate;
            x.LoadedFrom = "MenuViewModel";
            x.FromAddPerson = false;
        });
    }
}
