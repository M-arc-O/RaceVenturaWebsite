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

            if (_unitOfWork.VisitedPointRepository.Get().Any(vp => vp.PointId == entity.PointId && vp.TeamId == entity.TeamId))
            {
                throw new BusinessException($"Visited point with ID '{entity.PointId}' is already known", BLErrorCodes.Duplicate);
            }

            entity.Time = DateTime.Now;
            _unitOfWork.VisitedPointRepository.Insert(entity);
            _unitOfWork.Save();
        }

        public VisitedPoint Edit(Guid userId, VisitedPoint newEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid userId, Guid entityId)
        {
            var visitedPoint = _unitOfWork.VisitedPointRepository.GetByID(entityId);

            if (visitedPoint == null)
            {
                throw new BusinessException($"Visited point with ID '{entityId}' is unknown", BLErrorCodes.NotFound);
            }

            var team = GetTeam(visitedPoint.TeamId);
            CheckUserIsAuthorizedForRace(userId, team.RaceId, RaceAccessLevel.WriteTeams);

            _unitOfWork.VisitedPointRepository.Delete(visitedPoint);
            _unitOfWork.Save();
        }
    }
}
