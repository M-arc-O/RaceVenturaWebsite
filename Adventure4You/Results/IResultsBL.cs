
using System;
using System.Collections.Generic;
using Adventure4You.Models;
using Adventure4You.Models.Results;

namespace Adventure4You.Results
{
    public interface IResultsBL
    {
        BLReturnCodes GetRaceResults(Guid userId, Guid raceId, out List<TeamResult> teamResults);

        BLReturnCodes GetTeamResult(Guid userId, Guid raceId, Guid teamId, out TeamResult teamResult);
    }
}
