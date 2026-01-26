using Autofac;
using MovieDB.SharedModels;
using System.Windows;
using WPF_MovieDb.Models;
using WPF_MovieDb.Models.Interfaces;
using WPF_MovieDb.Services;
using WPF_MovieDb.Services.Interfaces;
using WPF_MovieDb.Services.Models;
using WPF_MovieDb.ViewModels;
using NavigationService = WPF_MovieDb.Services.NavigationService;

namespace WPF_MovieDb;


/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Setup container for dependency injection 
        ContainerBuilder builder = new();

        builder.RegisterType<HttpClientService>().SingleInstance();
        builder.RegisterType<APIService<Person>>().As<IAPIService<Person>>();
        builder.RegisterType<APIService<Movie>>().As<IAPIService<Movie>>();
        builder.RegisterType<PersonAPIService>().As<IPersonAPIService>();
        builder.RegisterType<MovieAPIService>().As<IMovieAPIService>();
        builder.RegisterType<UserAPIService>().As<IUserAPIService>();

        builder.RegisterType<GenerateInitialText>().As<IGenerateInitialText>();
        builder.RegisterType<StringsToMovieElements>().As<IStringsToMovieElements>();
        builder.RegisterType<MovieSearch>().As<IMovieSearch>();
        builder.RegisterType<UserHandling>();

        builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
        builder.RegisterType<MainViewModel>();
        builder.RegisterType<MenuViewModel>();
        builder.RegisterType<MovieEditorViewModel>();
        builder.RegisterType<ShowMovieDetailedViewModel>();
        builder.RegisterType<AddPersonViewModel>();
        builder.RegisterType<MovieSearchViewModel>();

        builder.RegisterType<ApplicationViewModel>();

        IContainer container = builder.Build();


        using (var scope = container.BeginLifetimeScope())
        {
            var navigationService = scope.Resolve<INavigationService>();

            navigationService.AddView(scope.Resolve<MainViewModel>())
                             .AddView(scope.Resolve<MovieEditorViewModel>())
                             .AddView(scope.Resolve<ShowMovieDetailedViewModel>())
                             .AddView(scope.Resolve<AddPersonViewModel>())
                             .AddView(scope.Resolve<MovieSearchViewModel>())
                             .AddView(scope.Resolve<MenuViewModel>());

            MainWindow = new MainWindow()
            {
                DataContext = scope.Resolve<ApplicationViewModel>()
            };
        }

        MainWindow.Show();
    }
}
