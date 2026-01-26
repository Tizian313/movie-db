using Database.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDB.SharedModels;
using System.Security.Claims;

namespace MovieDB.REST_API.Controllers;


[Authorize]
[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> logger;
    private readonly IMovieRepository repository;


    public MovieController(ILogger<MovieController> logger, IMovieRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }


    [HttpGet("Get/All")]
    public IEnumerable<Movie> GetAll()
    {
        logger.LogInformation("==> Got all movies.");
        return repository.GetAll();
    }

    [HttpGet("Get/ByID/{id}")]
    public Movie? Get(int id)
    {
        logger.LogInformation($"==> Got movie with id:{id}.");
        return repository.Get(id);
    }

    [HttpPost("Post")]
    public void Add(Movie movie)
    {
        // Set creator ID.
        movie.CreatorId = int.Parse( User.FindFirstValue(ClaimTypes.NameIdentifier)! );

        logger.LogInformation($"==> Added movie named \"{movie.Name}\".");
        repository.Add(movie);
    }

    [HttpDelete("Delete/{id}")]
    public void Remove(int id)
    {
        logger.LogInformation($"==> Removed movie with id:{id}.");
        repository.Remove(id);
    }
}
