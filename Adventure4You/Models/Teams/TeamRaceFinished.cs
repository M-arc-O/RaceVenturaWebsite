using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Teams
{
    [Table("TeamRacesFinished")]
    public class TeamRaceFinished
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeamRaceFinishedId { get; set; }

        [Required]
        public Guid RaceId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public DateTime StopTime { get; set; }
    }
}
