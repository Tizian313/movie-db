using CommunityToolkit.Mvvm.ComponentModel;
using MovieDB.SharedModels;
using WPF_MovieDb.Models.Interfaces;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Models;


public partial class StringsToMovieElements : ObservableObject, IStringsToMovieElements
{
    private readonly IPersonAPIService personAPIService;

    public StringsToMovieElements(IPersonAPIService personAPIService)
    {
        this.personAPIService = personAPIService;
    }

    public (string name, string errorMessage) GetName(string name)
    {
        string error = string.Empty;
        var trimmedName = name.Trim();

        if (trimmedName.Length == 0)
            error = "Name can't be empty.\n";

        return (trimmedName, error);
    }

    public (List<Genres> genres, string errorMessage) GetGenres(string genreText)
    {
        Genres validEnum;
        List<Genres> foundGenres = new();
        List<string> errors = new();

        List<string> inputs = genreText.Split(',').ToList<string>();


        foreach (var splitInput in inputs)
        {
            if (splitInput == "")
                continue;

            try
            {
                validEnum = (Genres)Enum.Parse(typeof(Genres), splitInput, true);
                if (!foundGenres.Contains(validEnum))
                    foundGenres.Add(validEnum);
            }
            catch
            { errors.Add(splitInput); }
        }


        string allErrors = string.Empty;

        if (errors.Count > 0)
        {
            foreach (var error in errors)
                allErrors += error + ", ";

            if (allErrors != string.Empty)
                allErrors = allErrors[..^2];

            allErrors = $"The genres: \"{allErrors}\" couldn't be parsed.\n";
        }

        return (foundGenres, allErrors);
    }

    public (List<Person> personList, bool errorHasOccured) GetPersons(string personText)
    {
        bool errorHasOccurred = false;
        List<Person> personListToReturn = new();

        // PersonText to fullNameList.
        List<string> fullNameList = personText.Split(',').ToList();

        foreach (var fullName in fullNameList)
        {
            string formattedName = fullName.ToLower().Trim();

            if (formattedName == string.Empty)
                break;

            Person? person = personAPIService.Get(formattedName);

            if (person == null) // If no matching person was found, a person is created to be later displayed in the person editor.
            {
                var stranger = CreateStranger(fullName);

                if (errorHasOccurred)
                    personListToReturn.Add(stranger);
                else
                {
                    personListToReturn = new() { stranger };
                    errorHasOccurred = true;
                }

                errorHasOccurred = true;
            }

            if (!errorHasOccurred)
                personListToReturn.Add(person!);
        }

        return (personListToReturn, errorHasOccurred);
    }

    public Person CreateStranger(string strangersName)
    {
        string[] formattedName = strangersName.Trim().Split(' ');
        string firstName = string.Empty;

        for (int i = 0; i < (formattedName.Length - 1); i++)
            firstName += formattedName[i] + ' ';

        firstName = firstName.Trim();

        Person stranger = new()
        {
            FirstName = firstName,
            LastName = formattedName.Last(),
            DateOfBirth = DateTime.Parse("01/01/1900"),
        };

        return stranger;
    }

    public (float rating, string errorMessage) GetRating(string ratingText)
    {
        float rating = 0.0f;
        string errorMessage = string.Empty;

        ratingText = ratingText.Replace('.', ',');

        try
        { rating = float.Parse(ratingText) / 10; }

        catch
        { errorMessage += "Rating couldn't be recognized as a decimal number.\n"; }


        if (rating > 10.0f)
            errorMessage += "Rating exceed the maximum value of \"10,0\".\n";

        if (rating < 0.0f)
            errorMessage += "Rating was less than the minimum value of \"0,0\".\n";

        // Restricts float to one decimal point.
        rating = (float) Math.Round(rating, 1);

        return (rating, errorMessage);
    }
}