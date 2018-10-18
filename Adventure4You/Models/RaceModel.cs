using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("Races")]
    public class RaceModel
    {
        [Key]
        public int RaceId { get; set; }

        [MaxLength(255)]
        public string RaceName { get; set; }

        public bool RaceCoordinatesCheckEnabled { get; set; }

        public RaceModel()
        {
        }
    }
}
