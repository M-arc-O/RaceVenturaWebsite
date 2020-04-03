using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Stages;
using Adventure4YouData.Models.Teams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        protected List<Race> GetRaces()
        {
            return _Context.Races
                .Include(r => r.Stages)
                .ThenInclude(s => s.Points)
                .Include(r => r.Teams)
                .ThenInclude(t => t.PointsVisited)
                .Include(r => r.Teams)
                .ThenInclude(t => t.StagesFinished).ToList();
        }

        protected Race GetRaceById(Guid raceId)
        {
            var race = GetRaces().FirstOrDefault(r => r.RaceId == raceId);
            return race;
        }

        protected Race GetRaceByStageId(Guid stageId)
        {
            return GetRaces().FirstOrDefault(r => r.Stages.Any(s => s.StageId == stageId));
        }

        protected Race GetRaceByTeamId(Guid teamId)
        {
            return GetRaces().FirstOrDefault(r => r.Teams.Any(t => t.TeamId == teamId));
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
