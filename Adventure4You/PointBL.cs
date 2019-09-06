using System;
using System.Collections.Generic;
using System.Linq;
using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Points;

namespace Adventure4You
{
    public class PointBL : BaseBL, IPointBL
    {
        public PointBL(IAdventure4YouDbContext context) : base(context)
        {

        }

        public BLReturnCodes GetPoint(Guid userId, Guid stageId, out List<Point> points)
        {
            points = null;
            var stage = _Context.Stages.FirstOrDefault(s => s.Id == stageId);

            if (stage == null)
            {
                return BLReturnCodes.UnknownStage;
            }            

            if (CheckIfUserHasAccessToRace(userId, stage.RaceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            points = _Context.Points.Where(point => point.StageId == stageId).ToList();

            if (points == null)
            {
                return BLReturnCodes.NoPointsFound;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes AddPoint(Guid userId, Point point)
        {
            var stage = _Context.Stages.FirstOrDefault(s => s.Id == point.StageId);

            if (CheckIfUserHasAccessToRace(userId, stage.RaceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            _Context.Points.Add(point);
            _Context.SaveChanges();

            return BLReturnCodes.Ok;
        }
    }
}
