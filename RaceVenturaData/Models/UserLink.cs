using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceVenturaData.Models
{
    [Table("UserLinks")]
    public class UserLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid RaceId { get; set; }

        [Required]
        public RaceAccessLevel RaceAccess { get; set; }

        public UserLink()
        {
        }
    }
}
