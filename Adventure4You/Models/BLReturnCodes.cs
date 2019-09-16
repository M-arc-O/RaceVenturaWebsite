using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure4You.Models
{

    public enum BLReturnCodes
    {
        Ok = 0,
        Duplicate = 1,
        UserUnauthorized = 2,
        NotFound = 3,
        Unknown = 4,
    }
}
