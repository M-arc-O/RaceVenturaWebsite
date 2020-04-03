using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4YouData.Models.Teams
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
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public List<TeamPointVisited> PointsVisited { get; set; }

        public List<TeamStageFinished> StagesFinished { get; set; }

        public DateTime RaceFinished { get; set; }

        public Team()
        {
        }
    }
}
