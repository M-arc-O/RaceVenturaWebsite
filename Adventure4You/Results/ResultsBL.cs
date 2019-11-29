using System;
using System.Collections.Generic;
using System.Linq;
using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Results;
using Adventure4You.Models.Stages;
using Adventure4You.Models.Teams;

namespace Adventure4You.Results
{
    public class ResultsBL : BaseBL, IResultsBL
    {
        public ResultsBL(IAdventure4YouDbContext context) : base(context)
        {

        }

        public BLReturnCodes GetRaceResults(Guid userId, Guid raceId, out List<TeamResult> teamResults)
        {
            teamResults = null;

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            var race = GetRaceById(raceId);
            if (race == null)
            {
                return BLReturnCodes.Unknown;
            }

            teamResults = new List<TeamResult>();
            foreach (var team  in race.Teams)
            {
                teamResults.Add(GetTeamResult(race, team));
            };

            teamResults = teamResults.OrderByDescending(r => r.NumberOfStages).ThenByDescending(r => r.TotalValue).ThenBy(r => r.EndTime).ToList();

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes GetTeamResult(Guid userId, Guid raceId, Guid teamId, out TeamResult teamResult)
        {
            teamResult = null;

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            var race = GetRaceById(raceId);
            var team = GetTeamById(teamId);
            if (race == null || team == null)
            {
                return BLReturnCodes.Unknown;
            }

            teamResult = GetTeamResult(race, team);

            return BLReturnCodes.Ok;
        }

        private TeamResult GetTeamResult(Race race, Team team)
        {
            var retVal = new TeamResult();
            retVal.TeamNumber = team.Number;
            retVal.TeamName = team.Name;
            retVal.NumberOfStages = 0;
            retVal.TotalValue = 0;
            retVal.EndTime = team.RaceFinished;
            retVal.NumberOfPoints = 0;
            retVal.StageResults = new List<StageResult>();

            foreach (var stage in race.Stages)
            {
                StageResult stageResult = GetStageResult(team, stage);

                var numberOfPointsToCompleteStage = stage.MimimumPointsToCompleteStage.HasValue ? stage.MimimumPointsToCompleteStage.Value : race.MinimumPointsToCompleteStage;
                retVal.NumberOfStages += stageResult.NumberOfPoints >= numberOfPointsToCompleteStage ? 1 : 0;
                retVal.TotalValue += stageResult.TotalValue;
                retVal.NumberOfPoints += stageResult.NumberOfPoints;
                retVal.StageResults.Add(stageResult);
            }

            SettleFinishTime(race, team, retVal);

            return retVal;
        }

        private static void SettleFinishTime(Race race, Team team, TeamResult retVal)
        {
            if (team.RaceFinished.CompareTo(race.EndTime) > 0)
            {
                var endTime = team.RaceFinished;
                if (team.RaceFinished.Second > 0)
                {
                    endTime = new DateTime(team.RaceFinished.Year, team.RaceFinished.Month, team.RaceFinished.Day, team.RaceFinished.Hour, team.RaceFinished.Minute, 0);
                    endTime = endTime.AddMinutes(1);
                }

                var timeDif = race.EndTime - endTime;
                retVal.TotalValue += (int)timeDif.TotalMinutes * race.PenaltyPerMinuteLate;
            }
        }

        private static StageResult GetStageResult(Team team, Stage stage)
        {
            var stageResult = new StageResult();
            stageResult.StageNumber = stage.Number;
            stageResult.StageName = stage.Name;
            stageResult.TotalValue = 0;
            stageResult.PointResults = new List<PointResult>();
             
            foreach (var pointVisited in team.PointsVisited)
            {
                var point = stage.Points.FirstOrDefault(p => p.PointId == pointVisited.PointId);
                if (point != null)
                {
                    stageResult.PointResults.Add(new PointResult
                    {
                        Name = point.Name,
                        Value = point.Value
                    });
                    stageResult.TotalValue += point.Value;
                }
            }

            return stageResult;
        }
    }
}
