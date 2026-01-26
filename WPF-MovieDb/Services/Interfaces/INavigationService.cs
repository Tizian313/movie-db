using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF_MovieDb.Services.Models
{
    public interface INavigationService
    {
        Action<ObservableObject>? OnNavigate { get; set; }

        NavigationService AddView<T>(T observableObject) where T : ObservableObject;
        void Navigate<T>(Action<T>? onSwitch = null) where T : ObservableObject;
    }
}