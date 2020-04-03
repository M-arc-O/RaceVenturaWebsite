using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Points;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You.Points
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
                points = GetStageById(stageId)?.Points.Where(p => p.StageId == stageId).OrderBy(p => p.Name).ToList();

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
                point = GetStageById(stageId)?.Points.FirstOrDefault(p => p.PointId == pointId);

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

                GetStageById(point.StageId)?.Points.Add(point);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeletePoint(Guid userId, Guid pointId, Guid stageId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, stageId);
            if (retVal == BLReturnCodes.Ok)
            {
                var point = GetPointById(stageId, pointId);
                if (point == null)
                {
                    return BLReturnCodes.Unknown;
                }

                GetStageById(stageId)?.Points.Remove(point);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes EditPoint(Guid userId, Point pointNew)
        {
            var retVal = CheckIfUserHasAccessToRaceAndStage(userId, pointNew.StageId);
            if (retVal == BLReturnCodes.Ok)
            {
                var point = GetPointById(pointNew.StageId, pointNew.PointId);
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

        protected Point GetPointById(Guid stageId, Guid pointId)
        {
            var stage = GetStageById(stageId);

            if (stage == null)
            {
                return null;
            }

            return stage.Points.FirstOrDefault(p => p.PointId == pointId);
        }

        private BLReturnCodes CheckIfUserHasAccessToRaceAndStage(Guid userId, Guid stageId)
        {
            var race = GetRaceByStageId(stageId);

            if (race == null)
            {
                return BLReturnCodes.Unknown;
            }

            if (base.CheckIfUserHasAccessToRace(userId, race.RaceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            return BLReturnCodes.Ok;
        }

        private bool CheckIfPointNameExists(Point point)
        {
            var stage = GetStageById(point.StageId);
            return stage == null ? false : stage.Points.Where(p => p.StageId == point.StageId).Any(p => p.Name.ToUpper().Equals(point.Name.ToUpper()));
        }
    }
}
