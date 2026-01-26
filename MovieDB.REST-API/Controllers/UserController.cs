using Database.Repository;
using Database.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDB.REST_API.Services;
using MovieDB.SharedModels;
using System.Security.Claims;

namespace MovieDB.REST_API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly UserRepository repository;
    private readonly IConfiguration configuration;
    private readonly TokenProviderService tokenProvider;

    public UserController(ILogger<UserController> logger, UserRepository repository, IConfiguration configuration, TokenProviderService tokenProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.configuration = configuration;
        this.tokenProvider = tokenProvider;
    }

    [AllowAnonymous]
    [HttpGet("Get/ByUsername/{username}")] // Returns null, if no user was found.
    public User? GetByUsername(string username)
    {
        logger.LogInformation($"==> Got user with username \"{username}\".");

        var user = repository.Get(username);

        if (user == null)
            return null;

        // Remove sensitive information before returning
        user.LoginAttempts = 0;
        user.Password = "";

        return user;
    }

    [HttpGet("Get/ByID/{userID}")]
    public User? GetByID(int userID)
    {
        var user = repository.Get(userID);

        if (user == null)
            return null;

        logger.LogInformation($"==> Got user with username \"{user.Username}\".");

        // Remove sensitive information before returning
        user.LoginAttempts = 0;
        user.Password = "";

        return user;
    }

    [HttpGet("Get/ActiveUserID")]
    public int GetActiveUserID()
    {
        return int.Parse( User.FindFirstValue(ClaimTypes.NameIdentifier)! );
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public bool Register(UserData registerData) // Return AlreadyExists
    {
        string username = registerData.username;
        string password = registerData.password;

        // Checks if a user with this name already is in the database.
        if (repository.Get(username) != null)
        {
            logger.LogInformation($"==> Failed to register user with username \"{username}\".");
            return false;
        }

        // Registers user.
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password + configuration["Pepper"], 12);
        User user = new(username, hashedPassword);
        repository.Add(user);

        logger.LogInformation($"==> Registered user with username \"{username}\".");
        return true;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public string Login([FromBody] UserData loginData) // Returns JSON Web Token.
    {
        string username = loginData.username ?? "";
        string password = loginData.password ?? "";

        User? targetUser = repository.Get(username);

        // If user doesn't exist.
        if (targetUser == null)
        {
            logger.LogInformation($"==> Failed to login, because no user with the username \"{username}\" exists.");
            return "ERROR:This user isn't registered.";
        }

        // If user is locked or has 0 login attempts.
        if (targetUser.LoginAttempts <= 0)
        {
            logger.LogInformation($"==> Failed to login, because user with the username \"{username}\" has no login attempts left.");
            return "ERROR:Please contact an admin to unlock this account.";
        }

        // If password is incorrect.
        if (!BCrypt.Net.BCrypt.Verify(password + configuration["Pepper"], targetUser.Password))
        {
            logger.LogInformation($"==> Failed to login, because the password was incorrect.");

            // Decrement LoginAttempt-Counter
            repository.DecrementLoginAttemptCounter(targetUser);

            if (targetUser.LoginAttempts <= 1)
                return "ERROR:Please contact an admin to unlock this account.";

            return "ERROR:Incorrect password.";
        }

        // Reset login attempts left and return a JSON web token.
        logger.LogInformation($"==> Successful login of user \"{loginData.username}\".");
        repository.ResetLoginAttempts(targetUser.Id);
        return tokenProvider.Create(targetUser);
    }
}
