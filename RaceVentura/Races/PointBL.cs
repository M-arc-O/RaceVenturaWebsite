using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace RaceVentura.Races
{
    public class PointBL : StageBaseBL, IGenericCudBL<Point>
    {
        public PointBL(IRaceVenturaUnitOfWork unitOfWork, ILogger<PointBL> logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Point point)
        {
            GetAndCheckStage(userId, point.StageId);
            CheckIfPointNameExists(point);

            _UnitOfWork.PointRepository.Insert(point);
            _UnitOfWork.Save();
        }

        public void Edit(Guid userId, Point newEntity)
        {
            var point = GetAndCheckPoint(userId, newEntity.PointId);

            if (!point.Name.ToUpper().Equals(newEntity.Name.ToUpper()))
            {
                CheckIfPointNameExists(newEntity);
            }

            point.Name = newEntity.Name;
            point.Type = newEntity.Type;
            point.Value = newEntity.Value;
            point.Latitude = newEntity.Latitude;
            point.Longitude = newEntity.Longitude;
            point.Answer = newEntity.Answer;
            point.Message = newEntity.Message;

            _UnitOfWork.PointRepository.Update(point);
            _UnitOfWork.Save();
        }

        public void Delete(Guid userId, Guid pointId)
        {
            GetAndCheckPoint(userId, pointId);

            _UnitOfWork.PointRepository.Delete(pointId);
            _UnitOfWork.Save();
        }

        private Point GetAndCheckPoint(Guid userId, Guid pointId)
        {
            var point = GetPoint(pointId);
            GetAndCheckStage(userId, point.StageId);
            return point;
        }

        private Point GetPoint(Guid pointId)
        {
            var point = _UnitOfWork.PointRepository.GetByID(pointId);
            if (point == null)
            {
                throw new BusinessException($"Point with id '{pointId}' not found.", BLErrorCodes.NotFound);
            }

            return point;
        }

        private void CheckIfPointNameExists(Point point)
        {
            if (_UnitOfWork.PointRepository.Get(p => p.StageId == point.StageId).Any(p => p.Name.ToUpper().Equals(point.Name.ToUpper())))
            {
                throw new BusinessException($"A point with name '{point.Name}' already exists.", BLErrorCodes.Duplicate);
            }
        }
    }
}
