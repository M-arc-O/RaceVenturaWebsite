using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You.Races
{
    public class RaceBL : RaceBaseBL, IRaceBL
    {
        public RaceBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public IEnumerable<Race> Get(Guid userId)
        {
            var links = _UnitOfWork.UserLinkRepository.Get(link => link.UserId == userId);

            return _UnitOfWork.RaceRepository.Get(r => links.Any(link => link.RaceId == r.RaceId), r => r.OrderBy(race => race.Name));
        }

        public Race GetById(Guid userId, Guid raceId)
        {
            if (GetRaceUserLink(userId, raceId) == null)
            {
                _Logger.LogWarning($"Error in {typeof(Race)}: User with ID '{userId}' tried to access race with ID {raceId} but is not authorized.");
                throw new BusinessException($"Error in {typeof(Race)}: User with ID '{userId}' is not authorized for race with ID {raceId}.", BLErrorCodes.UserUnauthorized);
            }

            var race = _UnitOfWork.RaceRepository.Get(r => r.RaceId == raceId, null,
                "Teams,Teams.PointsVisited,Teams.StagesFinished," +
                "Stages,Stages.Points").FirstOrDefault();
            if (race == null)
            {
                throw new BusinessException($"Error in {typeof(Race)}: No race with ID {raceId} found.", BLErrorCodes.Unknown);
            }

            race.Teams = race.Teams.OrderBy(team => team.Number).ToList();
            race.Stages = race.Stages.OrderBy(stage => stage.Number).ToList();
            race.Stages.ForEach(stage => stage.Points = stage.Points.OrderBy(point => point.Name).ToList());

            return race;
        }

        public void Add(Guid userId, Race race)
        {
            CheckIfRaceNameExists(race.Name);
            _UnitOfWork.RaceRepository.Insert(race);
            _UnitOfWork.Save();

            _UnitOfWork.UserLinkRepository.Insert(new UserLink
            {
                RaceId = race.RaceId,
                UserId = userId
            });

            _UnitOfWork.SaveAsync();
        }

        public void Delete(Guid userId, Guid raceId)
        {
            var userLink = GetRaceUserLink(userId, raceId);
            if (userLink == null)
            {
                _Logger.LogWarning($"Error in {typeof(Race)}: User with ID '{userId}' tried to access race with ID {raceId} but is not authorized.");
                throw new BusinessException($"User with ID '{userId}' is not authorized for race with ID {raceId}.", BLErrorCodes.UserUnauthorized);
            }

            var race = _UnitOfWork.RaceRepository.GetByID(raceId);
            if (race == null)
            {
                throw new BusinessException($"Race with ID '{raceId}' is unknown.", BLErrorCodes.Unknown);
            }

            _UnitOfWork.UserLinkRepository.Delete(userLink);
            _UnitOfWork.RaceRepository.Delete(race);

            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, Race raceNew)
        {
            if (GetRaceUserLink(userId, raceNew.RaceId) == null)
            {
                _Logger.LogWarning($"Error in {typeof(Race)}: User with ID '{userId}' tried to access race with ID {raceNew.RaceId} but is not authorized.");
                throw new BusinessException($"User with ID '{userId}' is not authorized for race with ID {raceNew.RaceId}.", BLErrorCodes.UserUnauthorized);
            }

            var race = _UnitOfWork.RaceRepository.GetByID(raceNew.RaceId);
            if (race == null)
            {
                throw new BusinessException($"Race with ID '{raceNew.RaceId}' is unknown.", BLErrorCodes.Unknown);
            }

            if (!race.Name.ToUpper().Equals(raceNew.Name.ToUpper()))
            {
                CheckIfRaceNameExists(raceNew.Name);
            }

            race.Name = raceNew.Name;
            race.CoordinatesCheckEnabled = raceNew.CoordinatesCheckEnabled;
            race.SpecialTasksAreStage = raceNew.SpecialTasksAreStage;
            race.MaximumTeamSize = raceNew.MaximumTeamSize;
            race.MinimumPointsToCompleteStage = raceNew.MinimumPointsToCompleteStage;
            race.StartTime = raceNew.StartTime;
            race.EndTime = raceNew.EndTime;
            _UnitOfWork.SaveAsync();
        }

        private void CheckIfRaceNameExists(string name)
        {
            if (_UnitOfWork.RaceRepository.Get().Any(race => race.Name.ToUpper().Equals(name.ToUpper())))
            {
                throw new BusinessException($"A race with name '{name}' allready exsists", BLErrorCodes.Duplicate);
            }
        }
    }
}
