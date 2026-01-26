using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YourMovieDB.Models
{
    public class Selection
    {
        public string Name { get; set; }

        public Action? Redirect { get; set; }

        public Selection(string name, Action? redirect)
        {
            Name = name;
            Redirect = redirect;
        }
    }
}
