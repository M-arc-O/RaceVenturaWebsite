using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Adventure4You.Races
{
    public class VisitedPointBL : TeamBaseBL, IGenericCudBL<VisitedPoint>
    {
        public VisitedPointBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, VisitedPoint entity)
        {
            var team = GetTeam(entity.TeamId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId);

            if (_UnitOfWork.VisitedPointRepository.Get().Any(vp => vp.PointId == entity.PointId && vp.TeamId == entity.TeamId))
            {
                throw new BusinessException($"Visited point with ID '{entity.VisitedPointId}' is already known", BLErrorCodes.Duplicate);
            }

            _UnitOfWork.VisitedPointRepository.Insert(entity);
            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, VisitedPoint newEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid userId, Guid entityId)
        {
            var team = GetTeam(entityId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId);

            _UnitOfWork.TeamRepository.Delete(entityId);
            _UnitOfWork.SaveAsync();
        }
    }
}
