using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Adventure4You.Races
{
    public class PointBL : StageBaseBL, IGenericBL<Point>
    {
        public PointBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Point point)
        {
            var stage = GetStage(point.StageId);

            CheckUserIsAuthorizedForRace(userId, stage.RaceId);

            if (CheckIfPointNameExists(point))
            {
                throw new BusinessException($"A point with name '{point.Name}' allready excits.", BLErrorCodes.Duplicate);
            }

            _UnitOfWork.PointRepository.Insert(point);
            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, Point newEntity)
        {
            var point = GetPoint(newEntity.PointId);

            if (point.StageId != newEntity.StageId)
            {
                _Logger.LogWarning($"Error in {typeof(PointBL)}: User with ID '{userId}' tried to edit point with ID '{newEntity.PointId}' but the stage ID '{newEntity.StageId}' of this point differs from the stage ID '{point.StageId}' in the database");
                throw new BusinessException($"Point with id '{newEntity.PointId}' has a stage ID that differs from the stage ID in the database.", BLErrorCodes.Unknown);
            }

            var stage = GetStage(point.StageId);

            CheckUserIsAuthorizedForRace(userId, stage.RaceId);

            if (!point.Name.ToUpper().Equals(newEntity.Name.ToUpper()) && CheckIfPointNameExists(newEntity))
            {
                throw new BusinessException($"A point with name '{newEntity.Name}' allready exists.", BLErrorCodes.Duplicate);
            }

            point.Name = newEntity.Name;
            point.Type = newEntity.Type;
            point.Value = newEntity.Value;
            point.Latitude = newEntity.Latitude;
            point.Longitude = newEntity.Longitude;
            point.Answer = newEntity.Answer;
            point.Message = newEntity.Message;

            _UnitOfWork.PointRepository.Update(point);
            _UnitOfWork.SaveAsync();
        }

        public void Delete(Guid userId, Guid pointId)
        {
            var point = GetPoint(pointId);
            var stage = GetStage(point.StageId);

            CheckUserIsAuthorizedForRace(userId, stage.RaceId);

            _UnitOfWork.PointRepository.Delete(pointId);
            _UnitOfWork.SaveAsync();
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

        private bool CheckIfPointNameExists(Point point)
        {
            return _UnitOfWork.PointRepository.Get(p => p.StageId == point.StageId).Any(p => p.Name.ToUpper().Equals(point.Name.ToUpper()));
        }
    }
}
