using System;
using System.Collections.Generic;
using System.Linq;
using Adventure4You.Models;
using Adventure4You.Models.Results;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;

namespace Adventure4You.Races
{
    public class ResultsBL : RaceBaseBL, IResultsBL
    {
        public ResultsBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public IEnumerable<TeamResult> GetRaceResults(Guid userId, Guid raceId)
        {
            CheckUserIsAuthorizedForRace(userId, raceId);
            var race = GetRace(raceId);

            var teamResults = new List<TeamResult>();
            foreach (var team in race.Teams)
            {
                teamResults.Add(GetTeamResult(race, team));
            };

            return teamResults.OrderByDescending(r => r.NumberOfStages).ThenByDescending(r => r.TotalValue).ThenBy(r => r.EndTime).ToList();
        }

        public TeamResult GetTeamResult(Guid userId, Guid raceId, Guid teamId)
        {
            CheckUserIsAuthorizedForRace(userId, raceId);

            var race = GetRace(raceId);
            var team = race.Teams.FirstOrDefault(t => t.TeamId == teamId);
            if (race == null || team == null)
            {
                throw new BusinessException($"Error in {typeof(ResultsBL)}: Race with ID '{raceId}' or team with ID '{teamId}' not found.", BLErrorCodes.Unknown);
            }

            return GetTeamResult(race, team);
        }

        private Race GetRace(Guid raceId)
        {
            return _UnitOfWork.RaceRepository.Get(t => t.RaceId == raceId, null, "Teams,Stages,Stages.Points").FirstOrDefault();
        }

        private TeamResult GetTeamResult(Race race, Team team)
        {
            var retVal = new TeamResult();
            retVal.TeamNumber = team.Number;
            retVal.TeamName = team.Name;
            retVal.NumberOfStages = 0;
            retVal.TotalValue = 0;
            retVal.EndTime = team.FinishTime;
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
            if (team.FinishTime.CompareTo(race.EndTime) > 0)
            {
                var endTime = team.FinishTime;
                if (team.FinishTime.Second > 0)
                {
                    endTime = new DateTime(team.FinishTime.Year, team.FinishTime.Month, team.FinishTime.Day, team.FinishTime.Hour, team.FinishTime.Minute, 0);
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
             
            foreach (var pointVisited in team.VisitedPoints)
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
