using Adventure4You.Models;
using Adventure4You.Models.Points;
using System.Collections.Generic;

namespace Adventure4You
{
    public interface IPointBL
    {
        BLReturnCodes GetPoint(int stageId, out List<Point> points);
        BLReturnCodes AddPoint(Point point, int stageId);
    }
}
