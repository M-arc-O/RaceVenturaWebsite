using System;
using System.Collections.Generic;
using Adventure4You.Models;

namespace Adventure4You
{
    public interface IRaceBL
    {
        List<Race> GetAllRaces(Guid userId);

        BLReturnCodes GetRaceDetails(Guid id, Guid raceId, out Race raceModel);

        BLReturnCodes AddRace(Guid userId, Race raceModel);

        BLReturnCodes EditRace(Guid id, Race raceModel);

        BLReturnCodes DeleteRace(Guid id, Guid raceId);
    }
}