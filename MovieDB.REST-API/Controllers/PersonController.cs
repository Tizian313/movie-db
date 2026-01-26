using Database.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDB.SharedModels;
using System.Security.Claims;

namespace MovieDB.REST_API.Controllers;


[Authorize]
[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly ILogger<PersonController> logger;
    private readonly IPersonRepository repository;


    public PersonController(ILogger<PersonController> logger, IPersonRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }


    [HttpGet("Get/ByID/{id}")]
    public Person? Get(int id)
    {
        logger.LogInformation($"==> Got person with id:{id}.");
        return repository.Get(id);
    }

    [HttpGet("Get/ByName/{fullName}")]
    public Person? Get(string fullName)
    {
        logger.LogInformation($"==> Got person named \"{fullName}\".");
        return repository.Get(fullName);
    }

    [HttpGet("Get/All")]
    public IEnumerable<Person> GetAll()
    {
        logger.LogInformation("==> Got all persons.");
        return repository.GetAll();
    }

    [HttpPost("Post")]
    public void Add(Person person)
    {
        // Set creator ID.
        person.CreatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        logger.LogInformation($"==> Added person named \"{person.FirstName} {person.LastName}\".");
        repository.Add(person);
    }

    [HttpDelete("Delete/{id}")]
    public void Remove(int id)
    {
        logger.LogInformation($"==> Removed movie with id:{id}.");
        repository.Remove(id);
    }
}
