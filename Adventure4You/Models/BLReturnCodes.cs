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
        NoRacesFound = 3,
        UnknownRace = 4,
        NoStagesFound = 5,
        UnknownStage = 6,
        NoPointsFound = 7,
        UnknownPoint = 8,
    }
}
