using MovieDB.SharedModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Services;

public class UserAPIService : IUserAPIService
{
    private readonly HttpClientService httpClientService;

    public UserAPIService(HttpClientService httpClientService)
    {
        this.httpClientService = httpClientService;
    }

    public User? Get(string username)
    {
        var httpClient = httpClientService.CreateClient();

        // Gets HTTP response message and checks if the response succeeded.
        var response = httpClient.GetAsync($"User/Get/ByUsername/{username}").Result;
        response.EnsureSuccessStatusCode();

        // Converts response to desired type
        var jsonString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<User>(jsonString);
    }
    public User? Get(int userID)
    {
        var httpClient = httpClientService.CreateClient();

        var response = httpClient.GetAsync($"User/Get/ByID/{userID}").Result;
        response.EnsureSuccessStatusCode();

        var jsonString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<User>(jsonString) ?? new();
    }

    public void Register(string username, string password)
    {
        var httpClient = httpClientService.CreateClient();

        // Serialize class into a JSON String
        UserData registerData = new(username, password);
        var jsonString = JsonConvert.SerializeObject(registerData);

        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var responseMessage = httpClient.PostAsync($"User/Register", httpContent).Result;

        responseMessage.EnsureSuccessStatusCode();
    }

    public string Login(string username, string password)
    {
        var httpClient = httpClientService.CreateClient();

        // Create HttpContent
        UserData loginData = new(username ?? "", password ?? "");
        var jsonString = JsonConvert.SerializeObject(loginData);

        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var responseMessage = httpClient.PostAsync($"User/Login", httpContent).Result;


        // Create RequestMessage
        //var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"User/Login");
        //requestMessage.Content = JsonContent.Create(loginData);

        // Gets HTTP response message and checks if the response succeeded.
        //var responseMessage = httpClient.SendAsync(requestMessage).Result;
        //responseMessage.EnsureSuccessStatusCode();

        // Return token or error message (would lead with "ERROR:")
        return responseMessage.Content.ReadAsStringAsync().Result;
    }

    public string GetActiveUsersName()
    {
        var httpClient = httpClientService.CreateClient();

        var response = httpClient.GetAsync($"User/Get/ActiveUserID").Result;

        response.EnsureSuccessStatusCode();

        var jsonString = response.Content.ReadAsStringAsync().Result;

        int userID = int.Parse(jsonString);

        return Get(userID)!.Username;
    }
}
