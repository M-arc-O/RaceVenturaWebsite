using System.Collections.Generic;
using System.Linq;
using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Points;

namespace Adventure4You
{
    public class PointBL : IPointBL
    {
        private readonly IAdventure4YouDbContext _Context;

        public PointBL(IAdventure4YouDbContext context)
        {
            _Context = context;
        }

        public BLReturnCodes GetPoint(int stageId, out List<Point> points)
        {
            points = null;

            var pointLinks = _Context.PointLinks.Where(link => link.StageId == stageId);

            if (pointLinks == null || pointLinks.Count() == 0)
            {
                return BLReturnCodes.UnknownStage;
            }

            points = _Context.Points.Where(point => pointLinks.Any(link => link.StageId == point.Id)).ToList();

            if (points == null)
            {
                return BLReturnCodes.NoPointsFound;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes AddPoint(Point point, int stageId)
        {
            _Context.Points.Add(point);
            _Context.SaveChanges();

            var pointLink = new PointLink
            {
                PointId = point.Id,
                StageId = stageId
            };
            _Context.PointLinks.Add(pointLink);
            _Context.SaveChanges();

            return BLReturnCodes.Ok;
        }
    }
}
