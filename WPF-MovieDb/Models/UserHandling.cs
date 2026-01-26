using WPF_MovieDb.Services;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Models;


public class UserHandling
{
    private readonly IUserAPIService userAPIService;
    private readonly HttpClientService httpClientService;

    public const int minimumCharacters = 8;


    public UserHandling(IUserAPIService userAPIService, HttpClientService httpClientService) 
    {
        this.userAPIService = userAPIService;
        this.httpClientService = httpClientService;
    }


    public string TryRegister(string username, string password, string repeatedPassword) // Returns error text.
    {
        if (username == null || username == string.Empty)
            return "Please enter a username.";

        if (userAPIService.Get(username) != null)
            return "User already exists.";

        string errorText = CheckPassword(password, repeatedPassword);

        if (errorText == "") // If no error was found ( CheckPassword returned empty string )
            userAPIService.Register(username, password);

        return errorText;
    }

    public string TryLogin(string username, string password)
    {
        string response = userAPIService.Login(username, password);
        
        // Return error (marked with leading string "ERROR:")
        if (response.StartsWith("ERROR:"))
            return response.Substring(6);

        // Sets the JWT of all HttpClients
        httpClientService.AccessToken = response;

        return "";
    }

    string CheckPassword(string password, string repeatedPassword)
    {
        if (password != repeatedPassword)
            return "The passwords don't match.";

        if (password == null || password.Length < minimumCharacters)
            return $"The password needs to be at least {minimumCharacters} characters long.";

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return "The password needs to have a special character.";

        if (!password.Any(c => !char.IsAsciiLetterUpper(c)) || !password.Any(c => !char.IsAsciiLetterLower(c)))
            return "The password needs to have lowercase and uppercase letters.";

        return "";
    }
}
