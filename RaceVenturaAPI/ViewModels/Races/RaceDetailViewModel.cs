using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class RaceDetailViewModel: RaceViewModel
    {
        [Required]
        public bool CoordinatesCheckEnabled { get; set; }

        public double AllowedCoordinatesDeviation { get; set; }

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

        public byte[] Avatar { get; set; }

        [Required]
        [MaxLength(500)]
        public string PointInformationText { get; set; }

        public List<TeamViewModel> Teams { get; set; }

        public List<StageViewModel> Stages { get; set; }
    }
}
