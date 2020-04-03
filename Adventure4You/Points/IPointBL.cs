using Adventure4YouData.Models;
using Adventure4YouData.Models.Points;
using System;
using System.Collections.Generic;

namespace Adventure4You.Points
{
    public interface IPointBL
    {
        BLReturnCodes GetPoints(Guid userId, Guid stageId, out List<Point> points);
        BLReturnCodes GetPointDetails(Guid userId, Guid stageId, Guid pointId, out Point point);
        BLReturnCodes AddPoint(Guid userId, Point point);
        BLReturnCodes DeletePoint(Guid userId, Guid pointId, Guid stageId);
        BLReturnCodes EditPoint(Guid userId, Point pointNew);
    }
}
