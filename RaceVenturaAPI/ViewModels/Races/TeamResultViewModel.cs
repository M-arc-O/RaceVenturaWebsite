using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class TeamResultViewModel
    {
        public int TeamNumber { get; set; }
        public string TeamName { get; set; }
        public DateTime? EndTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan RaceDuration { get; set; }
        public int TotalValue { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfStages { get; set; }
        public List<StageResultViewModel> StageResults { get; set; }
    }
}
