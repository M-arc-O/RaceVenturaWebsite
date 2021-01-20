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

            retVal.StageResults = retVal.StageResults.OrderBy(stage => stage.StageNumber).ToList();

            return retVal;
        }

        private static void SettleFinishTime(Race race, Team team, TeamResult retVal)
        {
            if (team.FinishTime.HasValue)
            {
                switch (race.RaceType)
                {
                    case RaceType.Classic:
                        retVal.RaceDuration = team.FinishTime.Value - race.StartTime.Value;
                        break;

                    case RaceType.TimeLimit:
                        SetRaceRurationForNonClassic(team, retVal);
                        break;

                    case RaceType.NoTimeLimit:
                        SetRaceRurationForNonClassic(team, retVal);
                        break;

                    default:
                        throw new Exception($"Unknown raceid {race.RaceId}");
                }

                if ((race.RaceType == RaceType.Classic || race.RaceType == RaceType.TimeLimit) && race.MaxDuration.HasValue)
                {
                    var difference = retVal.RaceDuration - race.MaxDuration.Value;
                    if (difference.TotalSeconds > 0)
                    {
                        retVal.TotalValue -= (int)Math.Ceiling(difference.TotalMinutes) * race.PenaltyPerMinuteLate.Value;
                    }
                }
            }
        }

        private static void SetRaceRurationForNonClassic(Team team, TeamResult retVal)
        {
            var firstVisitedPoint = team.VisitedPoints.OrderBy(t => t.Time).FirstOrDefault();

            if (firstVisitedPoint != null)
            {
                retVal.RaceDuration = team.FinishTime.Value - firstVisitedPoint.Time;
            }
        }

        private static StageResult GetStageResult(Team team, Stage stage)
        {
            var stageResult = new StageResult
            {
                StageNumber = stage.Number,
                StageName = stage.Name,
                MaxNumberOfPoints = stage.Points.Count,
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
