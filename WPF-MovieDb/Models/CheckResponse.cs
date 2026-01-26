using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MovieDb.Models
{
    public enum CheckResponse
    {
        DifferentPasswords,
        Valid,
        ToShort,
        NoSpecialCharacter,
        NoCaseVariation
    }
}
