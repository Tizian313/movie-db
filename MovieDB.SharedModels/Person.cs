using MovieDB.SharedModels.Interfaces;

namespace MovieDB.SharedModels;


public class Person : IIdentifications
{
    public int Id { get; set; }
    public int CreatorId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Person(string firstName, string lastName, DateTime dateOfBirth, int creatorId)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        CreatorId = creatorId;
    }

    public Person() { }
}
