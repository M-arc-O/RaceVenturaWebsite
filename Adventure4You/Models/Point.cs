using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("Points")]
    public class Point
    {
        [Key]
        public int PointId { get; set; }

        [Required]
        public int PointRaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PointName { get; set; }

        [Required]
        public int PointValue { get; set; }

        [MaxLength(255)]
        public string PointCoordinates { get; set; }

        public Point()
        {
        }
    }
}
