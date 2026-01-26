using Azure.Core;
using System.Net.Http;

namespace WPF_MovieDb.Services
{
    // Singleton
    public class HttpClientService
    {
        public string? AccessToken { get; set; }

        public HttpClient CreateClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7083"),
            };

            if (AccessToken != null)
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

            return httpClient;
        }
    }
}
