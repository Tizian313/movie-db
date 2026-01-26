using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDB.SharedModels.Interfaces
{
    public interface IIdentifications
    {
        int Id { get; set; }
        int CreatorId { get; set; }
    }
}
