using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using RaceVenturaData.Models;

namespace RaceVentura.Races
{
    public class TeamBL : TeamBaseBL, IGenericCudBL<Team>
    {
        public TeamBL(IRaceVenturaUnitOfWork unitOfWork, ILogger<TeamBL> logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Team entity)
        {
            CheckIfRaceExsists(userId, entity.RaceId);
            CheckUserIsAuthorizedForRace(userId, entity.RaceId, RaceAccessLevel.WriteTeams);
            CheckIfTeamNameExists(entity);
            CheckIfTeamNumberExists(entity);

            entity.ActiveStage = 1;

            _unitOfWork.TeamRepository.Insert(entity);
            _unitOfWork.Save();
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
            team.Category = newEntity.Category;
            team.FinishTime = newEntity.FinishTime;

            _unitOfWork.TeamRepository.Update(team);
            _unitOfWork.Save();
        }

        public void Delete(Guid userId, Guid entityId)
        {
            GetAndCheckTeam(userId, entityId);

            _unitOfWork.TeamRepository.Delete(entityId);
            _unitOfWork.Save();
        }        

        private void CheckIfTeamNameExists(Team team)
        {
            if (_unitOfWork.TeamRepository.Get(t => t.RaceId == team.RaceId).Any(t => t.Name.ToUpper().Equals(team.Name.ToUpper())))
            {
                throw new BusinessException($"A team with name '{team.Name}' already exists.", BLErrorCodes.Duplicate);
            }
        }

        private void CheckIfTeamNumberExists(Team team)
        {
            if (_unitOfWork.TeamRepository.Get(t => t.RaceId == team.RaceId).Any(t => t.Number == team.Number))
            {
                throw new BusinessException($"A team with number '{team.Number}' already exists.", BLErrorCodes.Duplicate);
            }
        }

        private Team GetAndCheckTeam(Guid userId, Guid teamId)
        {
            var team = GetTeam(teamId);
            CheckIfRaceExsists(userId, team.RaceId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId, RaceAccessLevel.WriteTeams);
            return team;
        }
    }
}
