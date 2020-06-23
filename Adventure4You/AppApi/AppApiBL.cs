using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Adventure4You.AppApi
{
    public class AppApiBL : IAppApiBL
    {
        private readonly IAdventure4YouUnitOfWork _UnitOfWork;
        private readonly ILogger _Logger;

        public AppApiBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger)
        {
            _UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RegisterToRace(Guid raceId, Guid teamId, string uniqueId)
        {
            var race = GetRace(raceId);
            var team = GetTeam(teamId);

            if (team.RegisteredIds.Count >= race.MaximumTeamSize)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to registers to many ID's to team with id '{raceId}'.");
                throw new BusinessException($"Maximum of registered ID's reached.", BLErrorCodes.MaxIdsReached);
            }

            if (team.RegisteredIds.Any(id => id.TeamId == teamId && id.UniqueId == uniqueId))
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to registers the unique ID '{uniqueId}' twice.");
                throw new BusinessException($"Unique ID '{uniqueId}' allready registered.", BLErrorCodes.Duplicate);
            }

            _UnitOfWork.RegisteredIdRepository.Insert(new RegisteredId  { TeamId = teamId, UniqueId = uniqueId });
            _UnitOfWork.Save();
        }

        public string RegisterPoint(Guid raceId, Guid teamId, string uniqueId, Guid pointId, double latitude, double longitude, string answer)
        {
            throw new NotImplementedException();
        }

        public void RegisterStageEnd(Guid raceId, Guid teamId, string uniqueId, Guid stageId)
        {
            throw new NotImplementedException();
        }

        public void RegisterRaceEnd(Guid raceId, Guid teamId, string uniqueId)
        {
            throw new NotImplementedException();
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

        private Team GetTeam(Guid teamId)
        {
            var team = _UnitOfWork.TeamRepository.Get(t => t.TeamId == teamId, null,
                "Teams,Teams.VisitedPoints,Teams.FinishedStages").FirstOrDefault();
            if (team == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: Someone tried to access team with ID '{teamId}' but it does not exsist.");
                throw new BusinessException($"Team with ID '{teamId}' is unknown", BLErrorCodes.NotFound);
            }

            return team;
        }
    }
}
