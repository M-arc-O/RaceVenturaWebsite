using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Points;
using Adventure4You.Models.Stages;
using Adventure4You.Models.Teams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Adventure4You
{
    public abstract class BaseBL
    {
        protected readonly IAdventure4YouDbContext _Context;

        public BaseBL(IAdventure4YouDbContext context)
        {
            _Context = context;
        }

        protected Race GetRaceById(Guid raceId)
        {
            return _Context.Races
                .Include(r => r.Teams)
                .Include(r => r.Stages)
                .ThenInclude(t => t.Points)
                .FirstOrDefault(r => r.RaceId == raceId);
        }

        protected Race GetRaceByStageId(Guid stageId)
        {
            return _Context.Races.Include(r => r.Stages)
                .ThenInclude(t => t.Points)
                .FirstOrDefault(r => r.Stages.Any(s => s.StageId == stageId));
        }

        protected Race GetRaceByTeamId(Guid teamId)
        {
            return _Context.Races.Include(r => r.Teams)
                .FirstOrDefault(r => r.Teams.Any(t => t.TeamId == teamId));
        }

        protected Stage GetStageById(Guid stageId)
        {
            return GetRaceByStageId(stageId).Stages.FirstOrDefault(s => s.StageId == stageId);
        }

        protected Team GetTeamById(Guid teamId)
        {
            return GetRaceByTeamId(teamId).Teams.FirstOrDefault(t => t.TeamId == teamId);
        }

        protected UserLink CheckIfUserHasAccessToRace(Guid userId, Guid raceId)
        {
            return _Context.UserLinks.FirstOrDefault(link => link.UserId == userId && link.RaceId == raceId);
        }
    }
}
