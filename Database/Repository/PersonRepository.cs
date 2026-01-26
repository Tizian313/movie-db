using Database.Repository.Interfaces;
using MovieDB.SharedModels;

namespace Database.Repository;


public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    protected readonly YourMovieDBContext context;


    public PersonRepository(YourMovieDBContext context) : base(context)
    {
        this.context = context;
    }


    public Person? Get(string fullName) // Returns null, if no person was found.
    {
        return context.Set<Person>().FirstOrDefault(q => (q.FirstName.ToLower() + " " + q.LastName.ToLower()) == fullName);
    }
}
