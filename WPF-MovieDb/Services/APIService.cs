using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace WPF_MovieDb.Services;


public class APIService<TTable> : IAPIService<TTable> where TTable : class
{
    private readonly HttpClientService httpClientService;
    private readonly string typeName;

    public APIService(HttpClientService httpClientService)
    {
        this.httpClientService = httpClientService;
        this.typeName = typeof(TTable).Name;
    }

    public TTable? Get(int id)
    {
        var httpClient = httpClientService.CreateClient();

        // Gets HTTP response message and checks if the response succeeded.
        var response = httpClient.GetAsync($"{typeName}/Get/ByID/{id}").Result;
        response.EnsureSuccessStatusCode();

        // Converts response to desired type
        var jsonString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<TTable>(jsonString);
    }

    public virtual List<TTable> GetAll()
    {
        var httpClient = httpClientService.CreateClient();

        // Gets HTTP response message and checks if the response succeeded.
        var response = httpClient.GetAsync($"{typeName}/Get/All").Result;
        response.EnsureSuccessStatusCode();

        // Converts response to desired type
        var jsonString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<List<TTable>>(jsonString) ?? new();
    }

    public void Add(TTable entry)
    {
        var httpClient = httpClientService.CreateClient();

        // Serialize class into a JSON String
        var jsonString = JsonConvert.SerializeObject(entry);

        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var responseMessage = httpClient.PostAsync($"{typeName}/Post", httpContent).Result;

        responseMessage.EnsureSuccessStatusCode();
    }

    public void Remove(int id)
    {
        var httpClient = httpClientService.CreateClient();

        var responseMessage = httpClient.DeleteAsync($"{typeName}/Delete/{id}").Result;

        responseMessage.EnsureSuccessStatusCode();
    }
}
