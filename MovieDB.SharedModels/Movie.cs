using MovieDB.SharedModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDB.SharedModels;


public class Movie : IIdentifications
{
    public int Id { get; set; }
    public int CreatorId { get; set; }

    public string Name { get; set; }
    public List<Genres> Genres { get; set; }
    public List<int> Director_IDs { get; set; }
    public List<int> Star_IDs { get; set; }
    public float Rating { get; set; }
    public Movie(string name, List<Genres> gernes, List<int> director_ids, List<int> star_ids, float rating, int creatorId)
    {
        Name = name;
        Genres = gernes;
        Director_IDs = director_ids;
        Star_IDs = star_ids;
        Rating = rating;
        CreatorId = creatorId;
    }

    public Movie() { }
}
