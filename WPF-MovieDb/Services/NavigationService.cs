
using CommunityToolkit.Mvvm.ComponentModel;
using WPF_MovieDb.Services.Models;
using WPF_MovieDb.ViewModels;

namespace WPF_MovieDb.Services
{
    public class NavigationService : INavigationService
    {
        private Dictionary<string, ObservableObject> pageViewModels = new();

        public NavigationService() { }
        
        public NavigationService AddView<T>(T observableObject) where T : ObservableObject
        {
            pageViewModels.Add(observableObject.GetType().Name, observableObject);
            return this;
        }

        public Action<ObservableObject>? OnNavigate { get; set; }

        public void Navigate<T>(Action<T>? onSwitch = null) where T : ObservableObject
        {
            var name = typeof(T).Name;
            var model = pageViewModels[name];
            if (model is T m)
            {
                onSwitch?.Invoke(m);
                OnNavigate?.Invoke(m);
            }
        }
    }
}