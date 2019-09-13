using System;
using System.Collections.Generic;
using Adventure4You.Models;

namespace Adventure4You
{
    public interface IRaceBL
    {
        BLReturnCodes GetAllRaces(Guid userId, out List<Race> races);
        BLReturnCodes GetRaceDetails(Guid userId, Guid raceId, out Race race);
        BLReturnCodes AddRace(Guid userId, Race race);
        BLReturnCodes DeleteRace(Guid userId, Guid raceId);
        BLReturnCodes EditRace(Guid userId, Race raceNew);
    }
}