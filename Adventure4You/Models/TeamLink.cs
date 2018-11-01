using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("TeamLinks")]
    public class TeamLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public int RaceId { get; set; }

        public TeamLink()
        {
        }
    }
}
