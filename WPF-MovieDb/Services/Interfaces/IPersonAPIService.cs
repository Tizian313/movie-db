using MovieDB.SharedModels;

namespace WPF_MovieDb.Services.Interfaces;


public interface IPersonAPIService : IAPIService<Person>
{
    Person Get(string fullName);
}
