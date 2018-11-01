using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("UserLinks")]
    public class UserLink
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }

        public int TeamId { get; set; }
        
        public int RaceId { get; set; }

        public UserLink()
        {
        }
    }
}
