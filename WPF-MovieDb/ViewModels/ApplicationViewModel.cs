
using CommunityToolkit.Mvvm.ComponentModel;
using WPF_MovieDb.Services.Models;

namespace WPF_MovieDb.ViewModels
{
    public partial class ApplicationViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject currentPageViewModel;

        public ApplicationViewModel(INavigationService navigationService)
        {
            navigationService.OnNavigate = NavigateTo;
            navigationService.Navigate<MainViewModel>();
        }

        private void NavigateTo(ObservableObject observableObject)
        {
            CurrentPageViewModel = observableObject;
        }
    }
}