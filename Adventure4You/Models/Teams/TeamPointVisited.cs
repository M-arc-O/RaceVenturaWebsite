using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Teams
{
    [Table("TeamPointsVisited")]
    public class TeamPointVisited
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PointId { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public TeamPointVisited()
        {

        }
    }
}
