using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace RaceVentura.AppApi
{
    public class AppApiBL : IAppApiBL
    {
        private readonly IRaceVenturaUnitOfWork _UnitOfWork;
        private readonly ILogger _Logger;

        public AppApiBL(IRaceVenturaUnitOfWork unitOfWork, ILogger<AppApiBL> logger)
        {
            _UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string RegisterToRace(Guid raceId, Guid teamId, Guid uniqueId)
        {
            if (_UnitOfWork.RegisteredIdRepository.Get().Any(id => id.UniqueId == uniqueId))
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to registers the unique ID '{uniqueId}' twice.");
                throw new BusinessException($"Unique ID '{uniqueId}' allready registered.", BLErrorCodes.Duplicate);
            }

            var team = GetTeamByTeamId(teamId);
            var registeredIds = _UnitOfWork.RegisteredIdRepository.Get(id => id.TeamId == teamId).ToList();

            var race = GetRace(raceId);
            if (registeredIds.Count >= race.MaximumTeamSize)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to registers to many ID's to team with id '{raceId}'.");
                throw new BusinessException($"Maximum of registered ID's reached.", BLErrorCodes.MaxIdsReached);
            }

            _UnitOfWork.RegisteredIdRepository.Insert(new RegisteredId  { TeamId = teamId, UniqueId = uniqueId });
            _UnitOfWork.Save();

            return race.Name;
        }

        public Point RegisterPoint(Guid raceId, Guid uniqueId, Guid pointId, double latitude, double longitude, string answer)
        {
            var team = GetTeamByRegisteredId(uniqueId);
            var point = GetPoint(pointId);
            var stage = GetStage(point.StageId);

            if (team.ActiveStage != stage.Number)
            {
                throw new BusinessException($"Point with ID '{pointId}' not in active stage.", BLErrorCodes.NotActiveStage);
            }

            var race = GetRace(raceId);
            var dateNow = DateTime.Now;
            CheckTime(race, dateNow);
            CheckCoordinates(race, point, latitude, longitude);

            if (point.Type == PointType.QuestionCheckPoint)
            {
                if (string.IsNullOrEmpty(answer.Trim()))
                {
                    return point;
                }

                if (!point.Answer.ToLower().Trim().Equals(answer.ToLower().Trim()))
                {
                    throw new BusinessException($"Answer '{answer}' is incorrect.", BLErrorCodes.AnswerIncorrect);
                }
            }

            if (_UnitOfWork.VisitedPointRepository.Get(p => p.TeamId == team.TeamId && p.PointId == pointId).Any())
            {
                throw new BusinessException($"Point already registered '{pointId}'.", BLErrorCodes.Duplicate);
            }

            _UnitOfWork.VisitedPointRepository.Insert(new VisitedPoint { TeamId = team.TeamId, PointId = pointId, Time = dateNow });
            _UnitOfWork.Save();

            return point;
        }

        public void RegisterStageStart(Guid raceId, Guid uniqueId, Guid stageId)
        {
            var team = GetTeamByRegisteredId(uniqueId);

            if (_UnitOfWork.FinishedStageRepository.Get(s => s.TeamId == team.TeamId && s.StageId == stageId).Any())
            {
                throw new BusinessException($"Stage with ID '{stageId}' has already been started.", BLErrorCodes.NotActiveStage);
            }

            var previousActiveStageNumber = team.ActiveStage;

            var stage = GetStage(stageId);
            //if (team.ActiveStage != stage.Number)
            //{
            //    throw new BusinessException($"Stage with ID '{stageId}' is not the active stage.", BLErrorCodes.NotActiveStage);
            //}

            var dateNow = DateTime.Now;
            var race = GetRace(raceId);
            CheckTime(race, dateNow);

            team.ActiveStage = stage.Number;

            _UnitOfWork.TeamRepository.Update(team);

            if (previousActiveStageNumber >= 0)
            {
                var previousActiveStage = _UnitOfWork.StageRepository.Get(s => s.Number == previousActiveStageNumber).First();
                _UnitOfWork.FinishedStageRepository.Insert(new FinishedStage { TeamId = team.TeamId, StageId = previousActiveStage.StageId, FinishTime = dateNow });
            }
            _UnitOfWork.Save();
        }

        public void RegisterRaceEnd(Guid raceId, Guid uniqueId)
        {
            var team = GetTeamByRegisteredId(uniqueId);

            var dateNow = DateTime.Now;
            var race = GetRace(raceId);
            CheckTime(race, dateNow);

            team.FinishTime = dateNow;
            _UnitOfWork.TeamRepository.Update(team);
            _UnitOfWork.Save();
        }

        private Race GetRace(Guid raceId)
        {
            var race = _UnitOfWork.RaceRepository.GetByID(raceId);
            if (race == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access race with ID '{raceId}' but it does not exsist.");
                throw new BusinessException($"Race with ID '{raceId}' does not exsist.", BLErrorCodes.NotFound);
            }

            return race;
        }

        private Team GetTeamByTeamId(Guid teamId)
        {
            var team = _UnitOfWork.TeamRepository.GetByID(teamId);
            if (team == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access team with ID '{teamId}' but it does not exsist.");
                throw new BusinessException($"Team with ID '{teamId}' is unknown", BLErrorCodes.NotFound);
            }

            CheckIfTeamHasFinished(team);

            return team;
        }

        private Team GetTeamByRegisteredId(Guid uniqueId)
        {
            var registeredIds = _UnitOfWork.RegisteredIdRepository.Get(id => id.UniqueId.Equals(uniqueId));
            if (registeredIds == null || registeredIds.Count() < 0)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to register a point but the unique id '{uniqueId}' does not exsist.");
                throw new BusinessException($"RegisteredID with uniqueID '{uniqueId}' is unknown", BLErrorCodes.UserUnauthorized);
            }

            var team = _UnitOfWork.TeamRepository.GetByID(registeredIds.First().TeamId);
            if (team == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access team with ID '{registeredIds.First().TeamId}' but it does not exsist.");
                throw new BusinessException($"Team with ID '{registeredIds.First().TeamId}' is unknown", BLErrorCodes.NotFound);
            }

            CheckIfTeamHasFinished(team);

            return team;
        }

        private static void CheckIfTeamHasFinished(Team team)
        {
            if (team.FinishTime.HasValue)
            {
                throw new BusinessException($"Race is ended already for team '{team.TeamId}'", BLErrorCodes.RaceEnded);
            }
        }

        private Stage GetStage(Guid stageId)
        {
            var stage = _UnitOfWork.StageRepository.GetByID(stageId);
            if (stage == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access stage with id '{stageId}' but it does not exsist.");
                throw new BusinessException($"Stage with ID '{stageId}' not found.", BLErrorCodes.NotFound);
            }

            return stage;
        }

        private Point GetPoint(Guid pointId)
        {
            var point = _UnitOfWork.PointRepository.GetByID(pointId);
            if (point == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access point with id '{pointId}' but it does not exsist.");
                throw new BusinessException($"Point with ID '{pointId}' not found.", BLErrorCodes.NotFound);
            }

            return point;
        }

        private static void CheckCoordinates(Race race, Point point, double latitude, double longitude)
        {           
            if (race.CoordinatesCheckEnabled)
            {
                var d1 = point.Latitude * (Math.PI / 180.0);
                var num1 = point.Longitude * (Math.PI / 180.0);

                var d2 = latitude * (Math.PI / 180.0);
                var num2 = longitude * (Math.PI / 180.0) - num1;

                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

                var distance = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

                if (distance > race.AllowedCoordinatesDeviation)
                {
                    throw new BusinessException($"Coordinates incorrect, latitude '{latitude}', longitude '{longitude}'", BLErrorCodes.CoordinatesIncorrect);
                }
            }
        }

        private static void CheckTime(Race race, DateTime date)
        {
            if (race.RaceType == RaceType.Classic && date < race.StartTime)
            {
                throw new BusinessException($"Race not started yet", BLErrorCodes.RaceNotStarted);
            }
        }
    }
}
