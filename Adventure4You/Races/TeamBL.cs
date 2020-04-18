using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Adventure4You.Races
{
    public class TeamBL : TeamBaseBL, IGenericBL<Team>
    {
        public TeamBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Team entity)
        {
            CheckUserIsAuthorizedForRace(userId, entity.RaceId);

            _UnitOfWork.TeamRepository.Insert(entity);
            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, Team newEntity)
        {
            CheckIfRaceExsists(userId, newEntity.RaceId);
            CheckUserIsAuthorizedForRace(userId, newEntity.RaceId);

            var team = GetTeam(newEntity.TeamId);

            if (team.RaceId != newEntity.RaceId)
            {
                _Logger.LogWarning($"Error in {typeof(TeamBL)}: User with ID '{userId}' tried to edit team with ID '{newEntity.TeamId}' but the race ID '{newEntity.RaceId}' of this entity does not match the race ID '{team.RaceId}' of the stage in the database.");
                throw new BusinessException($"Team with ID '{newEntity.TeamId}' is not part of race with ID '{newEntity.RaceId}'", BLErrorCodes.Unknown);
            }

            if (!newEntity.Name.ToUpper().Equals(team.Name.ToUpper()))
            {
                CheckIfTeamNameExists(newEntity);
            }

            if (newEntity.Number != team.Number)
            {
                CheckIfTeamNumberExists(newEntity);
            }

            team.Name = newEntity.Name;
            team.Number = newEntity.Number;
            team.FinishTime = newEntity.FinishTime;

            _UnitOfWork.TeamRepository.Update(team);
            _UnitOfWork.SaveAsync();
        }

        public void Delete(Guid userId, Guid entityId)
        {
            var team = GetTeam(entityId);
            CheckIfRaceExsists(userId, team.RaceId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId);

            _UnitOfWork.TeamRepository.Delete(entityId);
            _UnitOfWork.SaveAsync();
        }        

        private bool CheckIfTeamNameExists(Team team)
        {
            return _UnitOfWork.TeamRepository.Get(t => t.RaceId == team.RaceId).Any(t => t.Name.ToUpper().Equals(team.Name.ToUpper()));
        }

        private bool CheckIfTeamNumberExists(Team team)
        {
            return _UnitOfWork.TeamRepository.Get(t => t.RaceId == team.RaceId).Any(t => t.Number == team.Number);
        }
    }
}
