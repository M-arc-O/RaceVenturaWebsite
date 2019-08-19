using Adventure4You.Models;
using Adventure4You.Models.Points;
using System;
using System.Collections.Generic;

namespace Adventure4You
{
    public interface IPointBL
    {
        BLReturnCodes GetPoint(Guid stageId, out List<Point> points);
        BLReturnCodes AddPoint(Point point, Guid stageId);
    }
}
