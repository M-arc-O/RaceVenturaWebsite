using System;
using System.Collections.Generic;

namespace RaceVentura.Models.Results
{
    public class TeamResult
    {
        public int TeamNumber { get; set; }
        public string TeamName { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan RaceDuration { get; set; }
        public int TotalValue { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfStages { get; set; }
        public List<StageResult> StageResults { get; set; }
    }
}
