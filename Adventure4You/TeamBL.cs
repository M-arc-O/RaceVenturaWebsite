using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Teams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure4You
{
    public class TeamBL: BaseBL, ITeamBL
    {
        public TeamBL(IAdventure4YouDbContext context) : base(context)
        {
        }

        public BLReturnCodes AddTeam(Guid userId, Team team, Guid raceId)
        {
            throw new NotImplementedException();
        }

        public BLReturnCodes DeleteTeam(Guid userId, Guid teamId, Guid raceId)
        {
            throw new NotImplementedException();
        }

        public BLReturnCodes EditTeam(Guid userId, Team team)
        {
            throw new NotImplementedException();
        }

        public BLReturnCodes GetTeamDetails(Guid userId, Guid stageId, Guid raceId, out Team team)
        {
            throw new NotImplementedException();
        }

        public BLReturnCodes GetTeams(Guid userId, Guid raceId, out List<Team> teams)
        {
            throw new NotImplementedException();
        }
    }
}
