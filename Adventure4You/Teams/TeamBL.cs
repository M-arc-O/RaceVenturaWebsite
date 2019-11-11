using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You.Teams
{
    public class TeamBL : BaseBL, ITeamBL
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
                teams = GetRaceById(raceId)?.Teams?.Where(team => team.RaceId == raceId)?.OrderBy(team => team.Number)?.ToList();
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
                team = GetTeamById(teamId);
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
                if (CheckIfTeamNameExists(team) || CheckIfTeamNumberExists(team))
                {
                    return BLReturnCodes.Duplicate;
                }

                GetRaceById(raceId)?.Teams.Add(team);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeleteTeam(Guid userId, Guid teamId, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var team = GetTeamById(teamId);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }

                GetRaceByTeamId(teamId)?.Teams.Remove(team);
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
                var team = GetTeamById(teamNew.TeamId);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }

                if ((!teamNew.Name.ToUpper().Equals(team.Name.ToUpper()) && CheckIfTeamNameExists(teamNew)) ||
                    (teamNew.Number != team.Number && CheckIfTeamNumberExists(teamNew)))
                {
                    return BLReturnCodes.Duplicate;
                }

                team.Name = teamNew.Name;
                team.Number = teamNew.Number;
                team.RaceFinished = teamNew.RaceFinished;
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes PointVisited(Guid userId, TeamPointVisited model)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, model.RaceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var team = GetTeamById(model.TeamId);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }

                if (team.PointsVisited.Any(point => point.PointId == model.PointId))
                {
                    return BLReturnCodes.Duplicate;
                }

                team.PointsVisited.Add(model);

                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeleteTeamPointVisited(Guid userId, Guid teamId, Guid teamPointVisitedId, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var team = GetTeamById(teamId);
                if (team == null)
                {
                    return BLReturnCodes.Unknown;
                }

                var pointVisited = team.PointsVisited.FirstOrDefault(point => point.TeamPointVisitedId == teamPointVisitedId);
                if (pointVisited == null)
                {
                    return BLReturnCodes.Unknown;
                }

                team.PointsVisited.Remove(pointVisited);
                _Context.SaveChanges();

                return BLReturnCodes.Ok;
            }

            return retVal;
        }

        private BLReturnCodes CheckIfUserHasAccessToRaceAndRaceExists(Guid userId, Guid raceId)
        {
            var race = _Context.Races.Where(r => r.RaceId == raceId);
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

        private bool CheckIfTeamNameExists(Team team)
        {
            var race = GetRaceById(team.RaceId);
            return race == null ? false : race.Teams.Any(t => t.Name.ToUpper().Equals(team.Name.ToUpper()));
        }

        private bool CheckIfTeamNumberExists(Team team)
        {
            var race = GetRaceById(team.RaceId);
            return race == null ? false : race.Teams.Any(t => t.Number == team.Number);
        }
    }
}
