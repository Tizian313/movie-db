using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Http.Features;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPF_MovieDb.Models;
using WPF_MovieDb.Services;
using WPF_MovieDb.Services.Interfaces;
using WPF_MovieDb.Services.Models;

namespace WPF_MovieDb.ViewModels
{
    public partial class MainViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IUserAPIService userAPIService;
        private readonly UserHandling userHandling;

        public MainViewModel(INavigationService navigationService, IUserAPIService userAPIService, UserHandling userHandling)
        {
            this.Exit = new DelegateCommand((_) => System.Windows.Application.Current.Shutdown());
            this.Enter = new DelegateCommand((_) => OnEnter());

            this.isLoading = "Hidden";
            this.isNewUser = false;
            this.defaultButtonText = " Login ";
            this.errorText = "";

            this.navigationService = navigationService;
            this.userAPIService = userAPIService;
            this.userHandling = userHandling;
        }

        public ICommand Enter { get; set; }
        public ICommand Exit { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username {  get; set; }
        public string Password { private get; set; }
        public string RepeatedPassword { private get; set; }

        string squish;
        public string Squish 
        {
            get => squish;
            set => squish = value;
        }

        string isLoading;
        public string IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                this.RaisePropertyChanged();
            }
        }

        bool isNewUser;
        public bool IsNewUser
        {
            get => isNewUser;
            set
            {
                isNewUser = value;

                if (value)
                    DefaultButtonText = " Register ";
                else
                    DefaultButtonText = " Login ";

                this.RaisePropertyChanged();
            }
        }

        string defaultButtonText;
        public string DefaultButtonText
        {
            get => defaultButtonText;
            set
            {
                defaultButtonText = value;
                this.RaisePropertyChanged();
            }
        }

        string errorText;
        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
                this.RaisePropertyChanged();
            }
        }

        async void OnEnter()
        {
            ErrorText = "";

            // Show loading indication
            IsLoading = "Visible";
            await Task.Run(async () => await Task.Delay(1)); // Update UI

            // Registers if selected to
            if (IsNewUser)
                ErrorText = userHandling.TryRegister(Username, Password, RepeatedPassword);

            // Login if no error has been thrown.
            if (ErrorText == "")
                ErrorText = userHandling.TryLogin(Username, Password);

            // Hide loading indicator.
            IsLoading = "Hidden";

            if (ErrorText == "")
                navigationService.Navigate<MenuViewModel>();
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (!string.IsNullOrEmpty(propertyName))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
