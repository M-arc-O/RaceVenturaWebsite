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
    public class RaceBL : RaceBaseBL, IGenericCrudBL<Race>
    {
        public RaceBL(IAdventure4YouUnitOfWork unitOfWork, ILogger<RaceBL> logger) : base(unitOfWork, logger)
        {

        }

        public IEnumerable<Race> Get(Guid userId)
        {
            var links = _UnitOfWork.UserLinkRepository.Get(link => link.UserId == userId);

            return _UnitOfWork.RaceRepository.Get(r => links.Any(link => link.RaceId == r.RaceId), r => r.OrderBy(race => race.Name));
        }

        public Race GetById(Guid userId, Guid raceId)
        {
            GetAndCheckUserLink(userId, raceId);

            var race = _UnitOfWork.RaceRepository.Get(r => r.RaceId == raceId, null,
                "Teams,Teams.VisitedPoints,Teams.FinishedStages," +
                "Stages,Stages.Points").FirstOrDefault();
            if (race == null)
            {
                throw new BusinessException($"No race with ID {raceId} found.", BLErrorCodes.NotFound);
            }

            race.Teams = race.Teams.OrderBy(team => team.Number).ToList();
            race.Stages = race.Stages.OrderBy(stage => stage.Number).ToList();
            race.Stages.ForEach(stage => stage.Points = stage.Points?.OrderBy(point => point.Name).ToList());

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

            _UnitOfWork.Save();
        }

        public void Edit(Guid userId, Race newEntity)
        {
            CheckIfRaceExsists(userId, newEntity.RaceId);
            CheckUserIsAuthorizedForRace(userId, newEntity.RaceId);

            var race = _UnitOfWork.RaceRepository.GetByID(newEntity.RaceId);

            if (!race.Name.ToUpper().Equals(newEntity.Name.ToUpper()))
            {
                CheckIfRaceNameExists(newEntity.Name);
            }

            race.Name = newEntity.Name;
            race.CoordinatesCheckEnabled = newEntity.CoordinatesCheckEnabled;
            race.AllowedCoordinatesDeviation = newEntity.AllowedCoordinatesDeviation;
            race.PenaltyPerMinuteLate = newEntity.PenaltyPerMinuteLate;
            race.SpecialTasksAreStage = newEntity.SpecialTasksAreStage;
            race.MaximumTeamSize = newEntity.MaximumTeamSize;
            race.MinimumPointsToCompleteStage = newEntity.MinimumPointsToCompleteStage;
            race.StartTime = newEntity.StartTime;
            race.EndTime = newEntity.EndTime;

            _UnitOfWork.RaceRepository.Update(race);

            _UnitOfWork.Save();
        }

        public void Delete(Guid userId, Guid raceId)
        {
            CheckIfRaceExsists(userId, raceId);
            UserLink userLink = GetAndCheckUserLink(userId, raceId);

            _UnitOfWork.UserLinkRepository.Delete(userLink);
            _UnitOfWork.RaceRepository.Delete(raceId);

            _UnitOfWork.Save();
        }

        private UserLink GetAndCheckUserLink(Guid userId, Guid raceId)
        {
            var userLink = GetRaceUserLink(userId, raceId);
            if (userLink == null)
            {
                _Logger.LogWarning($"Error in {nameof(RaceBL)}: User with ID '{userId}' tried to access race with ID {raceId} but is not authorized.");
                throw new BusinessException($"User is not authorized for race.", BLErrorCodes.UserUnauthorized);
            }

            return userLink;
        }

        private void CheckIfRaceNameExists(string name)
        {
            if (_UnitOfWork.RaceRepository.Get().Any(race => race.Name.ToUpper().Equals(name.ToUpper())))
            {
                throw new BusinessException($"A race with name '{name}' already exists.", BLErrorCodes.Duplicate);
            }
        }
    }
}
