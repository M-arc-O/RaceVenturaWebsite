using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class RaceDetailViewModel: RaceViewModel
    {
        [Required]
        public bool CoordinatesCheckEnabled { get; set; }

        [Required]
        public bool SpecialTasksAreStage { get; set; }

        [Required]
        public int MaximumTeamSize { get; set; }

        [Required]
        public int MinimumPointsToCompleteStage { get; set; }

        [Required]
        public int PenaltyPerMinuteLate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public List<TeamViewModel> Teams { get; set; }

        public List<StageViewModel> Stages { get; set; }
    }
}
