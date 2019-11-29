using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Results
{
    public class StageResultViewModel
    {
        public int StageNumber { get; set; }
        public string StageName { get; set; }
        public int TotalValue { get; set; }
        public int NumberOfPoints { get; set; }
        public List<PointResultViewModel> PointResults { get; set; }
    }
}
