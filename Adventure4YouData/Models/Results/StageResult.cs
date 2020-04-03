
using System.Collections.Generic;

namespace Adventure4YouData.Models.Results
{
    public class StageResult
    {
        public int StageNumber { get; set; }
        public string StageName { get; set; }
        public int TotalValue { get; set; }
        public int NumberOfPoints
        {
            get
            {
                return PointResults == null ? 0 : PointResults.Count;
            }
        }
        public List<PointResult> PointResults { get; set; }
    }
}
