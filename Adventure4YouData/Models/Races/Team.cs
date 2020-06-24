using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4YouData.Models.Races
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
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public TeamCategory Category { get; set; }

        public List<RegisteredId> RegisteredIds { get; set; }

        public List<VisitedPoint> VisitedPoints { get; set; }

        public List<FinishedStage> FinishedStages { get; set; }

        public DateTime FinishTime { get; set; }

        public int ActiveStage { get; set; }

        public Team()
        {
        }
    }
}
