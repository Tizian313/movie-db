using MovieDB.SharedModels;
using WPF_MovieDb.Services.Interfaces;

namespace WPF_MovieDb.Services;


public class MovieAPIService : APIService<Movie>, IMovieAPIService
{
    private readonly HttpClientService httpClientService;

    public MovieAPIService(HttpClientService httpClientService) : base(httpClientService)
    {
        this.httpClientService = httpClientService;
    }
}
