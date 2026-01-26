using MovieDB.SharedModels;

namespace WPF_MovieDb.Services.Interfaces
{
    public interface IUserAPIService
    {
        User? Get(int userID);
        User? Get(string username);
        string GetActiveUsersName();
        string Login(string username, string password);
        void Register(string username, string password);
    }
}