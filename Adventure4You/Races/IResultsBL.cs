using System;
using System.Collections.Generic;
using Adventure4You.Models.Results;

namespace Adventure4You.Races
{
    public interface IResultsBL
    {
        IEnumerable<TeamResult> GetRaceResults(Guid userId, Guid raceId);

        TeamResult GetTeamResult(Guid userId, Guid raceId, Guid teamId);
    }
}
