using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4YouData.Models.Races
{
    [Table("Races")]
    public class Race
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RaceId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

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

        public List<Stage> Stages { get; set; }

        public List<Team> Teams { get; set; }

        public Race()
        {
        }
    }
}
