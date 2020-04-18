using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class RaceDetailViewModel: RaceViewModel
    {
        public bool CoordinatesCheckEnabled { get; set; }
        public bool SpecialTasksAreStage { get; set; }
        public int MaximumTeamSize { get; set; }
        public int MinimumPointsToCompleteStage { get; set; }
        public int PenaltyPerMinuteLate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<TeamViewModel> Teams { get; set; }
        public List<StageViewModel> Stages { get; set; }
    }
}
