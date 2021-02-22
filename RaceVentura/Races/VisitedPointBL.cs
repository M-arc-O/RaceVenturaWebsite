using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using RaceVenturaData.Models;

namespace RaceVentura.Races
{
    public class VisitedPointBL : TeamBaseBL, IGenericCudBL<VisitedPoint>
    {
        public VisitedPointBL(IRaceVenturaUnitOfWork unitOfWork, ILogger<VisitedPointBL> logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, VisitedPoint entity)
        {
            var team = GetTeam(entity.TeamId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId, RaceAccessLevel.WriteTeams);

            if (_UnitOfWork.VisitedPointRepository.Get().Any(vp => vp.PointId == entity.PointId && vp.TeamId == entity.TeamId))
            {
                throw new BusinessException($"Visited point with ID '{entity.PointId}' is already known", BLErrorCodes.Duplicate);
            }

            entity.Time = DateTime.Now;
            _UnitOfWork.VisitedPointRepository.Insert(entity);
            _UnitOfWork.Save();
        }

        public void Edit(Guid userId, VisitedPoint newEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid userId, Guid entityId)
        {
            var visitedPoint = _UnitOfWork.VisitedPointRepository.GetByID(entityId);

            if (visitedPoint == null)
            {
                throw new BusinessException($"Visited point with ID '{entityId}' is unknown", BLErrorCodes.NotFound);
            }

            var team = GetTeam(visitedPoint.TeamId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId, RaceAccessLevel.WriteTeams);

            _UnitOfWork.VisitedPointRepository.Delete(visitedPoint);
            _UnitOfWork.Save();
        }
    }
}
