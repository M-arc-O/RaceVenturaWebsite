using System;
using System.Collections.Generic;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Teams;

namespace Adventure4You.Teams

{
    public interface ITeamBL
    {
        BLReturnCodes GetTeams(Guid userId, Guid raceId, out List<Team> teams);
        BLReturnCodes GetTeamDetails(Guid userId, Guid stageId, Guid raceId, out Team team);
        BLReturnCodes AddTeam(Guid userId, Team team, Guid raceId);
        BLReturnCodes DeleteTeam(Guid userId, Guid teamId, Guid raceId);
        BLReturnCodes EditTeam(Guid userId, Team team);
        BLReturnCodes PointVisited(Guid userId, TeamPointVisited model);
        BLReturnCodes DeleteTeamPointVisited(Guid userId, Guid teamId, Guid teamPointVisitedId, Guid raceId);
    }
}
