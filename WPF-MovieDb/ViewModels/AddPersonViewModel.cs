using CommunityToolkit.Mvvm.ComponentModel;
using MovieDB.SharedModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPF_MovieDb.Models;
using WPF_MovieDb.Services.Interfaces;
using WPF_MovieDb.Services.Models;

namespace WPF_MovieDb.ViewModels;


public partial class AddPersonViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly INavigationService navigationService;
    private readonly IPersonAPIService personAPIService;

    public AddPersonViewModel(INavigationService navigationService, IPersonAPIService personAPIService)
    {
        this.Confirm = new DelegateCommand((_) => OnConfirm());
        this.Cancel = new DelegateCommand((_) => FinishAddPersonInstance());

        this.personAPIService = personAPIService;
        this.navigationService = navigationService;
    }

    public ICommand Confirm { get; set; }
    public ICommand Cancel { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;


    [ObservableProperty]
    private List<Person> personsLeft;

    [ObservableProperty]
    private string loadedFrom;

    string firstNameInput;
    public string FirstNameInput
    {
        get => firstNameInput;
        set { if (firstNameInput != value) firstNameInput = value; }
    }

    string lastNameInput;
    public string LastNameInput
    {
        get => lastNameInput;
        set { if (lastNameInput != value) lastNameInput = value; }
    }

    string ratingInput;
    public string DateInput
    {
        get => ratingInput;
        set { if (ratingInput != value) ratingInput = value; }
    }


    public string OnChangedToView
    {
        get
        {
            FirstNameInput = PersonsLeft[0].FirstName;
            LastNameInput = PersonsLeft[0].LastName;
            DateInput = PersonsLeft[0].DateOfBirth.ToString("dd.MM.yyyy"); ;
            return string.Empty;
        }
    }


    void OnConfirm()
    {
        // Assemble person and don't continue if inputs a invalid
        string errorText = string.Empty;

        PersonsLeft[0].FirstName = FirstNameInput.Trim();
        PersonsLeft[0].LastName = LastNameInput.Trim();

        if (PersonsLeft[0].FirstName == string.Empty || PersonsLeft[0].LastName == string.Empty)
            errorText += $"The name can't be empty.\n";

        try
            { PersonsLeft[0].DateOfBirth = DateTime.Parse(DateInput); }
        catch
            { errorText += $"\"{DateInput}\" can't be read as a date.\n Try using this example format: \"dd.mm.yyyy\"\nd = Day\nm = Month\ny = Year\n"; }

        if (DateTime.Now <= PersonsLeft[0].DateOfBirth)
            errorText += $"Invalid date of birth. \"{DateInput}\" is in the future.\n";

        if (errorText != string.Empty)
        {
            MessageBox.Show(errorText, "Input error.");
            return;
        }

        // Save person to database
        personAPIService.Add(PersonsLeft[0]);

        FinishAddPersonInstance();
    }


    void FinishAddPersonInstance()
    {
        // Removes person from PersonsLeft
        PersonsLeft.RemoveAt(0);

        // Load next AddPerson instance or return to MovieEditorViewModel
        if (PersonsLeft.Count > 0)
            navigationService.Navigate<AddPersonViewModel>(x =>  { x.PersonsLeft = PersonsLeft; });

        else
            navigationService.Navigate<MovieEditorViewModel>( x => { x.FromAddPerson = true; } );
    }
}
