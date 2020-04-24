using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class TeamViewModel
    {
        public Guid TeamId { get; set; }
        public Guid RaceId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<VisitedPointViewModel> VisitedPoints { get; set; }
        public DateTime FinishTime { get; set; }
        public List<FinishedStageViewModel> FinishedStages { get; set; }
    }
}
