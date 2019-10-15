using System;
using System.Collections.Generic;
using Adventure4You.Models;
using Adventure4You.Models.Teams;

namespace Adventure4You.Teams

{
    public interface ITeamBL
    {
        BLReturnCodes GetTeams(Guid userId, Guid raceId, out List<Team> teams);
        BLReturnCodes GetTeamDetails(Guid userId, Guid stageId, Guid raceId, out Team team);
        BLReturnCodes AddTeam(Guid userId, Team team, Guid raceId);
        BLReturnCodes DeleteTeam(Guid userId, Guid teamId, Guid raceId);
        BLReturnCodes EditTeam(Guid userId, Team team);
    }
}
