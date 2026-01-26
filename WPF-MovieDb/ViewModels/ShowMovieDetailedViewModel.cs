using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_MovieDb.Models.Items;
using WPF_MovieDb.Services.Models;
using WPF_MovieDb.Models;
using MovieDB.SharedModels;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.ViewModels;


public partial class ShowMovieDetailedViewModel : ObservableObject
{
    private readonly IUserAPIService userAPIService;
    private readonly IPersonAPIService personAPIService;
    private readonly IMovieAPIService movieAPIService;
    private readonly INavigationService navigationService;

    public ShowMovieDetailedViewModel(INavigationService navigationService, IPersonAPIService personAPIService, IMovieAPIService movieAPIService, IUserAPIService userAPIService)
    {
        this.Back = new DelegateCommand( (_) => navigationService.Navigate<MovieSearchViewModel>() );

        this.Edit = new DelegateCommand( (_) => navigationService.Navigate<MovieEditorViewModel>(
            x => { 
                     x.Movie = LoadedMovie; 
                     x.LoadedFrom = "ShowMovieDetailedViewModel";
                     x.FromAddPerson = false;
                 }));

        this.Delete = new DelegateCommand(
            (_) => {
                       movieAPIService.Remove(LoadedMovie.Id);
                       navigationService.Navigate<MovieSearchViewModel>();
                   });

        this.userAPIService = userAPIService;
        this.personAPIService = personAPIService;
        this.navigationService = navigationService;
    }

    public ICommand Back { get; set; }
    public ICommand Edit { get; set; }
    public ICommand Delete { get; set; }


    [ObservableProperty]
    private Movie loadedMovie;

    public string MovieName { get => $"{LoadedMovie.Name}"; }
    public string StarWidth { get => ((int)Math.Round(400 * (LoadedMovie.Rating / 10))).ToString(); }

    public string ActorCanvasHeight { get => (70 * LoadedMovie.Star_IDs.Count).ToString(); }
    public string DirectorCanvasHeight { get => (70 * LoadedMovie.Director_IDs.Count).ToString(); }
    public string GenreCanvasHeight { get => (70 * LoadedMovie.Genres.Count).ToString(); }
    public string CreatorLabel { get => GenerateCreatorLabel(); }
    

    public ObservableCollection<CanvasLabel> ActorItemsSource
    {
        get { return BuildPersonItemSource(LoadedMovie.Star_IDs); }
    }

    public ObservableCollection<CanvasLabel> DirectorItemsSource
    {
        get { return BuildPersonItemSource(LoadedMovie.Director_IDs); }
    }

    public ObservableCollection<CanvasLabel> GenreItemsSource
    {
        get { return BuildGenreItemSource(); }
    }


    string GenerateCreatorLabel()
    {
        User? user = userAPIService.Get(LoadedMovie.CreatorId);

        if (user.Username == null)
            return "Registered by: *Deleted*";

        return $"Registered by: {user.Username}";
    }


    ObservableCollection<CanvasLabel> BuildPersonItemSource(List<int> idList)
    {
        ObservableCollection<CanvasLabel> source = new();

        foreach (int id in idList)
        {
            Person? person = personAPIService.Get(id);

            if (person != null)
            {
                var age = DateTime.Today.Year - person.DateOfBirth.Year;
                source.Add( new() 
                { 
                    CanvasText = $"{person.FirstName} {person.LastName} {age}y/o" 
                });
            }
        }

        source.Add(new()
        {
            CanvasText = ""
        });
       
        return source;
    }

    ObservableCollection<CanvasLabel> BuildGenreItemSource()
    {
        ObservableCollection<CanvasLabel> source = new();

        foreach (Genres genre in LoadedMovie.Genres)
        {
            string? genreName = Enum.GetName(typeof(Genres), genre);

            source.Add(new()
            {
                CanvasText = $"{genreName}"
            });
        }

        source.Add(new()
        {
            CanvasText = ""
        });

        return source;
    }
}
