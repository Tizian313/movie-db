namespace MovieDB.SharedModels;


public class UserData
{
    public string username { get; set; }
    public string password {get; set; }

    public UserData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public UserData() { }
}
