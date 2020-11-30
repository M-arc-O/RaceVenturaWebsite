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

        public void RegisterToRace(Guid raceId, Guid teamId, string uniqueId)
        {
            var team = GetTeamByTeamId(teamId);
            var registeredIds = _UnitOfWork.RegisteredIdRepository.Get(id => id.TeamId == teamId).ToList();

            var race = GetRace(raceId);
            if (registeredIds.Count >= race.MaximumTeamSize)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to registers to many ID's to team with id '{raceId}'.");
                throw new BusinessException($"Maximum of registered ID's reached.", BLErrorCodes.MaxIdsReached);
            }

            if (registeredIds.Any(id => id.UniqueId == uniqueId))
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to registers the unique ID '{uniqueId}' twice.");
                throw new BusinessException($"Unique ID '{uniqueId}' allready registered.", BLErrorCodes.Duplicate);
            }

            _UnitOfWork.RegisteredIdRepository.Insert(new RegisteredId  { TeamId = teamId, UniqueId = uniqueId });
            _UnitOfWork.Save();
        }

        public string RegisterPoint(Guid raceId, string uniqueId, Guid pointId, double latitude, double longitude, string answer)
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

            if (!string.IsNullOrEmpty(point.Message))
            {
                if (string.IsNullOrEmpty(answer.Trim()))
                {
                    return point.Message;
                }

                if (!point.Answer.Equals(answer.Trim()))
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

            return "";
        }

        public void RegisterStageEnd(Guid raceId, string uniqueId, Guid stageId)
        {
            var team = GetTeamByRegisteredId(uniqueId);

            var stage = GetStage(stageId);
            if (team.ActiveStage != stage.Number)
            {
                throw new BusinessException($"Stage with ID '{stageId}' is not the active stage.", BLErrorCodes.NotActiveStage);
            }

            var dateNow = DateTime.Now;
            var race = GetRace(raceId);
            CheckTime(race, dateNow);

            team.ActiveStage++;

            _UnitOfWork.TeamRepository.Update(team);
            _UnitOfWork.FinishedStageRepository.Insert(new FinishedStage { TeamId = team.TeamId, StageId = stageId, FinishTime = dateNow });
            _UnitOfWork.Save();
        }

        public void RegisterRaceEnd(Guid raceId, string uniqueId)
        {
            var team = GetTeamByRegisteredId(uniqueId);

            GetRace(raceId);

            team.FinishTime = DateTime.Now;
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

            return team;
        }

        private Team GetTeamByRegisteredId(string uniqueId)
        {
            var registeredId = _UnitOfWork.RegisteredIdRepository.Get(id => id.UniqueId.Equals(uniqueId)).FirstOrDefault();
            if (registeredId == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access registeredId with uniqueID '{uniqueId}' but it does not exsist.");
                throw new BusinessException($"RegisteredID with uniqueID '{uniqueId}' is unknown", BLErrorCodes.NotFound);
            }

            var team = _UnitOfWork.TeamRepository.GetByID(registeredId.TeamId);
            if (team == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access team with ID '{registeredId.TeamId}' but it does not exsist.");
                throw new BusinessException($"Team with ID '{registeredId.TeamId}' is unknown", BLErrorCodes.NotFound);
            }

            return team;
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
            if (race.CoordinatesCheckEnabled && (
                latitude < point.Latitude - race.AllowedCoordinatesDeviation ||
                latitude > point.Latitude + race.AllowedCoordinatesDeviation ||
                longitude < point.Longitude - race.AllowedCoordinatesDeviation ||
                longitude > point.Longitude + race.AllowedCoordinatesDeviation))
            {
                throw new BusinessException($"Coordinates incorrect, latitude '{latitude}', longitude '{longitude}'", BLErrorCodes.CoordinatesIncorrect);
            }
        }

        private static void CheckTime(Race race, DateTime date)
        {
            if (date < race.StartTime)
            {
                throw new BusinessException($"Race not started yet", BLErrorCodes.RaceNotStarted);
            }
        }
    }
}
