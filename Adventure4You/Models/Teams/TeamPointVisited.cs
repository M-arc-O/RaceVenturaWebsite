using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Teams
{
    [Table("TeamPointsVisited")]
    public class TeamPointVisited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid PointId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public Guid StageId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public TeamPointVisited()
        {

        }
    }
}
