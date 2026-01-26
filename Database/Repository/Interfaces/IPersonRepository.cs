using MovieDB.SharedModels;

namespace Database.Repository.Interfaces;


public interface IPersonRepository : IBaseRepository<Person>
{
    Person? Get(string fullName);
}
