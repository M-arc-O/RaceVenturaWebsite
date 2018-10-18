using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("Races")]
    public class RaceModel
    {
        [Key]
        public int RaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string RaceName { get; set; }

        [Required]
        public bool RaceCoordinatesCheckEnabled { get; set; }

        public RaceModel()
        {
        }
    }
}
