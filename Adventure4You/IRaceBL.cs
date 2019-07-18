using System.Collections.Generic;
using Adventure4You.Models;

namespace Adventure4You
{
    public interface IRaceBL
    {
        List<Race> GetAllRaces();

        BLReturnCodes GetRaceDetails(string id, int raceId, out Race raceModel);

        BLReturnCodes AddRace(string userId, Race raceModel);

        BLReturnCodes EditRace(string id, Race raceModel);

        BLReturnCodes DeleteRace(string id, int raceId);
    }
}