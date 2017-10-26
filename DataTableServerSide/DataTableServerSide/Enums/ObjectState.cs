using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.Enums
{
    public enum ObjectState
    {
        NotSet = 0,
        Added = 1,
        Updated = 2,
        Deleted = 3,
        NotChanged = 4
    }
}
