using Database.Repository.Interfaces;
using MovieDB.SharedModels;

namespace Database.Repository;


public class UserRepository : IUserRepository
{
    protected readonly YourMovieDBContext context;


    public UserRepository(YourMovieDBContext context)
    {
        this.context = context;
    }


    public User? Get(string userName) // Returns null, if no person was found.
    {
        return context.Set<User>().FirstOrDefault(q => q.Username == userName.Trim());
    }

    public User? Get(int id)
    {
        return context.Set<User>().FirstOrDefault(q => q.Id == id);
    }

    public void Add(User user)
    {
        context.Set<User>().Add(user);
        context.SaveChanges();
    }

    public void DecrementLoginAttemptCounter(User targetUser)
    {
        var user = context.Set<User>().FirstOrDefault(q => q.Id == targetUser.Id);

        user!.LoginAttempts--;

        context.SaveChanges();
    }

    public void ResetLoginAttempts(int id)
    {
        User originalUser = Get(id)!;

        originalUser.LoginAttempts = 5;

        context.Set<User>().Update(originalUser);
        context.SaveChanges();
    }
}
