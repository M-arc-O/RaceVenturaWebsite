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

            _unitOfWork.PointRepository.Insert(point);
            _unitOfWork.Save();
        }

        public Point Edit(Guid userId, Point newEntity)
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

            _unitOfWork.PointRepository.Update(point);
            _unitOfWork.Save();

            return point;
        }

        public void Delete(Guid userId, Guid pointId)
        {
            GetAndCheckPoint(userId, pointId);

            _unitOfWork.PointRepository.Delete(pointId);
            _unitOfWork.Save();
        }

        private Point GetAndCheckPoint(Guid userId, Guid pointId)
        {
            var point = GetPoint(pointId);
            GetAndCheckStage(userId, point.StageId);
            return point;
        }

        private Point GetPoint(Guid pointId)
        {
            var point = _unitOfWork.PointRepository.GetByID(pointId);
            if (point == null)
            {
                throw new BusinessException($"Point with id '{pointId}' not found.", BLErrorCodes.NotFound);
            }

            return point;
        }

        private void CheckIfPointNameExists(Point point)
        {
            if (_unitOfWork.PointRepository.Get(p => p.StageId == point.StageId).Any(p => p.Name.ToUpper().Equals(point.Name.ToUpper())))
            {
                throw new BusinessException($"A point with name '{point.Name}' already exists.", BLErrorCodes.Duplicate);
            }
        }
    }
}
