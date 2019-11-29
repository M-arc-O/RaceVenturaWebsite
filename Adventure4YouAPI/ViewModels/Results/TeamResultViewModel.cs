using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Results
{
    public class TeamResultViewModel
    {
        public int TeamNumber { get; set; }
        public string TeamName { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalValue { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfStages { get; set; }
        public List<StageResultViewModel> StageResults { get; set; }
    }
}
