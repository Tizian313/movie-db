using MovieDB.SharedModels;
using Newtonsoft.Json;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Services;


public class PersonAPIService : APIService<Person>, IPersonAPIService
{
    private readonly HttpClientService httpClientService;

    public PersonAPIService(HttpClientService httpClientService) : base(httpClientService)
    {
        this.httpClientService = httpClientService;
    }

    public Person Get(string fullName) // Use .ToLower().Trim() to make the string more readable
    {
        var httpClient = httpClientService.CreateClient();

        // Gets HTTP response message and checks if the response succeeded.
        var response = httpClient.GetAsync($"Person/Get/ByName/{fullName}").Result;
        response.EnsureSuccessStatusCode();

        // Converts response to desired type
        var jsonString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<Person>(jsonString) ?? new();
    }
}
