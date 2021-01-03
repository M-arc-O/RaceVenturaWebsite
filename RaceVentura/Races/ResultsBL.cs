using System;
using System.Collections.Generic;
using System.Linq;
using RaceVentura.Models;
using RaceVentura.Models.Results;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;

namespace RaceVentura.Races
{
    public class ResultsBL : RaceBaseBL, IResultsBL
    {
        public ResultsBL(IRaceVenturaUnitOfWork unitOfWork, ILogger<ResultsBL> logger) : base(unitOfWork, logger)
        {

        }

        public IEnumerable<TeamResult> GetRaceResults(Guid raceId)
        {
            var race = GetRace(raceId);

            var teamResults = new List<TeamResult>();
            foreach (var team in race.Teams)
            {
                teamResults.Add(GetTeamResult(race, team));
            };

            return teamResults.OrderByDescending(r => r.NumberOfStages).ThenByDescending(r => r.TotalValue).ThenBy(r => r.RaceDuration).ToList();
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

        private static TeamResult GetTeamResult(Race race, Team team)
        {
            var retVal = new TeamResult
            {
                TeamNumber = team.Number,
                TeamName = team.Name,
                NumberOfStages = 0,
                TotalValue = 0,
                EndTime = team.FinishTime,
                NumberOfPoints = 0,
                StageResults = new List<StageResult>()
            };

            foreach (var stage in race.Stages)
            {
                StageResult stageResult = GetStageResult(team, stage);

                var numberOfPointsToCompleteStage = stage.MimimumPointsToCompleteStage ?? race.MinimumPointsToCompleteStage;

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
            if (race.RaceType == RaceType.Classic && team.FinishTime.HasValue)
            {
                retVal.RaceDuration = team.FinishTime.Value - race.StartTime.Value;

                if (team.FinishTime.Value.CompareTo(race.EndTime) > 0)
                {
                    var endTime = team.FinishTime;
                    if (team.FinishTime.Value.Second > 0)
                    {
                        endTime = new DateTime(team.FinishTime.Value.Year, team.FinishTime.Value.Month, team.FinishTime.Value.Day, team.FinishTime.Value.Hour, team.FinishTime.Value.Minute, 0);
                        endTime = endTime.Value.AddMinutes(1);
                    }

                    var timeDif = race.EndTime - endTime;
                    retVal.TotalValue += (int)timeDif.Value.TotalMinutes * race.PenaltyPerMinuteLate.Value;
                }
            }
            else
            {
                var firstVisitedPointTime = team.VisitedPoints.OrderBy(t => t.Time).FirstOrDefault();
                
                if (team.FinishTime.HasValue && firstVisitedPointTime != null)
                {
                    retVal.RaceDuration = team.FinishTime.Value - firstVisitedPointTime.Time;
                }
            }
        }

        private static StageResult GetStageResult(Team team, Stage stage)
        {
            var stageResult = new StageResult
            {
                StageNumber = stage.Number,
                StageName = stage.Name,
                TotalValue = 0,
                PointResults = new List<PointResult>()
            };

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
