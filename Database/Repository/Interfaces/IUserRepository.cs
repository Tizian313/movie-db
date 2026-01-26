using MovieDB.SharedModels;

namespace Database.Repository.Interfaces;


public interface IUserRepository
{
    User? Get(string userName);
    User? Get(int id);

    void Add(User user);

    void DecrementLoginAttemptCounter(User targetUser);

    void ResetLoginAttempts(int id);
}
