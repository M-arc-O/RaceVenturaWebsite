using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Points;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You
{
    public class PointBL : BaseBL, IPointBL
    {
        public PointBL(IAdventure4YouDbContext context) : base(context)
        {

        }

        public BLReturnCodes GetPoints(Guid userId, Guid stageId, out List<Point> points)
        {
            points = null;

            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, stageId);
            if (retVal == BLReturnCodes.Ok)
            {
                points = _Context.Points.Where(p => p.StageId == stageId).OrderBy(p => p.Name).ToList();

                if (points == null)
                {
                    return BLReturnCodes.NotFound;
                }
            }

            return retVal;
        }

        public BLReturnCodes GetPointDetails(Guid userId, Guid stageId, Guid pointId, out Point point)
        {
            point = null;

            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, stageId);
            if (retVal == BLReturnCodes.Ok)
            {
                point = _Context.Points.FirstOrDefault(p => p.Id == pointId);

                if (point == null)
                {
                    return BLReturnCodes.Unknown;
                }
            }

            return retVal;
        }

        public BLReturnCodes AddPoint(Guid userId, Point point)
        {
            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, point.StageId);
            if (retVal == BLReturnCodes.Ok)
            {
                if (CheckIfPointNameExists(point))
                {
                    return BLReturnCodes.Duplicate;
                }

                _Context.Points.Add(point);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeletePoint(Guid userId, Guid pointId, Guid stageId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, stageId);
            if (retVal == BLReturnCodes.Ok)
            {
                var point = _Context.Points.FirstOrDefault(p => p.Id == pointId);
                if (point == null)
                {
                    return BLReturnCodes.Unknown;
                }

                _Context.Points.Remove(point);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes EditPoint(Guid userId, Point pointNew)
        {
            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, pointNew.StageId);
            if (retVal == BLReturnCodes.Ok)
            {
                var point = _Context.Points.FirstOrDefault(p => p.Id == pointNew.Id);
                if (point == null)
                {
                    return BLReturnCodes.Unknown;
                }

                if (!point.Name.ToUpper().Equals(pointNew.Name.ToUpper()) && CheckIfPointNameExists(pointNew))
                {
                    return BLReturnCodes.Duplicate;
                }

                point.Name = pointNew.Name;
                point.Type = pointNew.Type;
                point.Value = pointNew.Value;
                point.Latitude = pointNew.Latitude;
                point.Longitude = pointNew.Longitude;
                point.Answer = pointNew.Answer;
                point.Message = pointNew.Message;

                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes RemovePoints(Guid userId, Guid stageId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, stageId);
            if (retVal == BLReturnCodes.Ok)
            {
                var points = _Context.Points.Where(point => point.StageId == stageId);
                _Context.Points.RemoveRange(points);
                _Context.SaveChanges();
            }

            return retVal;
        }

        private BLReturnCodes CheckIfUserHasAccessToRaceAndStage(Guid userId, Guid stageId)
        {
            var stage = _Context.Stages.FirstOrDefault(s => s.Id == stageId);

            if (stage == null)
            {
                return BLReturnCodes.Unknown;
            }

            if (base.CheckIfUserHasAccessToRace(userId, stage.RaceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            return BLReturnCodes.Ok;
        }

        private bool CheckIfPointNameExists(Point point)
        {
            return _Context.Points.Where(p => p.StageId == point.StageId).Any(p=>p.Name.ToUpper().Equals(point.Name.ToUpper()));
        }
    }
}
