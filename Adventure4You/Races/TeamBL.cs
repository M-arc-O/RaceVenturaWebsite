using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Adventure4You.Races
{
    public class TeamBL : TeamBaseBL, IGenericCudBL<Team>
    {
        public TeamBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Team entity)
        {
            CheckIfRaceExsists(userId, entity.RaceId);
            CheckUserIsAuthorizedForRace(userId, entity.RaceId);
            CheckIfTeamNameExists(entity);
            CheckIfTeamNumberExists(entity);

            _UnitOfWork.TeamRepository.Insert(entity);
            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, Team newEntity)
        {
            Team team = GetAndCheckTeam(userId, newEntity.TeamId);

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
            GetAndCheckTeam(userId, entityId);

            _UnitOfWork.TeamRepository.Delete(entityId);
            _UnitOfWork.SaveAsync();
        }        

        private void CheckIfTeamNameExists(Team team)
        {
            if (_UnitOfWork.TeamRepository.Get(t => t.RaceId == team.RaceId).Any(t => t.Name.ToUpper().Equals(team.Name.ToUpper())))
            {
                throw new BusinessException($"A team with name '{team.Name}' already exists.", BLErrorCodes.Duplicate);
            }
        }

        private void CheckIfTeamNumberExists(Team team)
        {
            if (_UnitOfWork.TeamRepository.Get(t => t.RaceId == team.RaceId).Any(t => t.Number == team.Number))
            {
                throw new BusinessException($"A team with number '{team.Number}' already exists.", BLErrorCodes.Duplicate);
            }
        }

        private Team GetAndCheckTeam(Guid userId, Guid teamId)
        {
            var team = GetTeam(teamId);
            CheckIfRaceExsists(userId, team.RaceId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId);
            return team;
        }
    }
}
