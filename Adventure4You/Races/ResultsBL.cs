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
            var race = GetRace(raceId);

            CheckUserIsAuthorizedForRace(userId, raceId);

            var teamResults = new List<TeamResult>();
            foreach (var team in race.Teams)
            {
                teamResults.Add(GetTeamResult(race, team));
            };

            return teamResults.OrderByDescending(r => r.NumberOfStages).ThenByDescending(r => r.TotalValue).ThenBy(r => r.EndTime).ToList();
        }

        private Race GetRace(Guid raceId)
        {
            var race = _UnitOfWork.RaceRepository.Get(t => t.RaceId == raceId, null, "Teams,Teams.VisitedPoints,Stages,Stages.Points").FirstOrDefault();

            if (race == null)
            {
                throw new BusinessException($"Race with ID '{raceId}' not found.", BLErrorCodes.NotFound);
            }

            return race;
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

                if (stageResult.NumberOfPoints >= numberOfPointsToCompleteStage)
                {
                    retVal.NumberOfStages += 1;
                    retVal.TotalValue += stageResult.TotalValue;
                    retVal.NumberOfPoints += stageResult.NumberOfPoints;
                }
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
