using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceVenturaData.Models.Races
{
    [Table("VisitedPoints")]
    public class VisitedPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid VisitedPointId { get; set; }

        [Required]
        public Guid PointId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}
