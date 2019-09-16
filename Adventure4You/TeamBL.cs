﻿using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adventure4You
{
    public class TeamBL: BaseBL, ITeamBL
    {
        public TeamBL(IAdventure4YouDbContext context) : base(context)
        {
        }

        public BLReturnCodes GetTeams(Guid userId, Guid raceId, out List<Team> teams)
        {
            teams = null;

            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                teams = _Context.Teams.Where(team => team.RaceId == raceId).OrderBy(team => team.Number).ToList();
                if (teams == null)
                {
                    return BLReturnCodes.NotFound;
                }
            }

            return retVal;
        }

        public BLReturnCodes GetTeamDetails(Guid userId, Guid teamId, Guid raceId, out Team team)
        {
            team = null;

            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                team = GetTeam(teamId);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }
            }

            return retVal;
        }

        public BLReturnCodes AddTeam(Guid userId, Team team, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                if (CheckIfTeamNameAndNumberExists(team))
                {
                    return BLReturnCodes.Duplicate;
                }

                _Context.Teams.Add(team);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeleteTeam(Guid userId, Guid teamId, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var team = GetTeam(teamId);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }
                
                _Context.Teams.Remove(team);
                _Context.SaveChanges();

                return BLReturnCodes.Ok;
            }

            return retVal;
        }

        public BLReturnCodes EditTeam(Guid userId, Team teamNew)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, teamNew.RaceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var team = GetTeam(teamNew.Id);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }

                if ((!teamNew.Name.ToUpper().Equals(teamNew.Name.ToUpper()) || teamNew.Number != team.Number) && CheckIfTeamNameAndNumberExists(teamNew))
                {
                    return BLReturnCodes.Duplicate;
                }

                team.Name = teamNew.Name;
                team.Number = teamNew.Number;
                _Context.SaveChanges();
            }

            return retVal;
        }

        private Team GetTeam(Guid teamId)
        {
            return _Context.Teams.FirstOrDefault(t => t.Id == teamId);
        }

        private BLReturnCodes CheckIfUserHasAccessToRaceAndRaceExists(Guid userId, Guid raceId)
        {
            var race = _Context.Races.Where(r => r.Id == raceId);
            if (race == null || race.Count() == 0)
            {
                return BLReturnCodes.Unknown;
            }

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            return BLReturnCodes.Ok;
        }

        private bool CheckIfTeamNameAndNumberExists(Team team)
        {
            return _Context.Teams.Where(t => t.RaceId == team.RaceId).Any(t => t.Name.ToUpper().Equals(team.Name.ToUpper())) ||
                _Context.Teams.Where(t=>t.RaceId == team.RaceId).Any(t=>t.Number == team.Number);
        }
    }
}
