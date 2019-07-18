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
        UnknownRace = 3,
        NoStagesFound = 4,
        UnknownStage = 5,
        NoPointsFound = 6,
    }
}
