using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Teams
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeamId { get; set; }

        [Required]
        public Guid RaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        public List<TeamPhone> RegisteredPhoneIds { get; set; }

        public List<TeamPointVisited> PointsVisited { get; set; }

        public List<TeamStageFinished> StagesFinished { get; set; }

        public TeamRaceFinished RaceFinished { get; set; }

        public Team()
        {
        }
    }
}
