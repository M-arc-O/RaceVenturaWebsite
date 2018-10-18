using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("UserLinks")]
    public class UserLink
    {
        [Key]
        public int UserLinkId { get; set; }
        
        [Required]
        public int UserLinkUserId { get; set; }

        public int UserLinkTeamId { get; set; }
        
        public int UserLinkRaceId { get; set; }

        public UserLink()
        {
        }
    }
}
