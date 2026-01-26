using MovieDB.SharedModels.Interfaces;

namespace MovieDB.SharedModels;


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int LoginAttempts { get; set; }
    public List<int> Watchlist { get; set; }

    public User(string username, string password, List<int> watchlist = default, int loginAttempts = 5)
    {
        Username = username;
        Password = password;
        Watchlist = watchlist ?? new();
        LoginAttempts = loginAttempts;
    }

    public User() { }
}
