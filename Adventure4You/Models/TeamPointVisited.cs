using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.Models
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
