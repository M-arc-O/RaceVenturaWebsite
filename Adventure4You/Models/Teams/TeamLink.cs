using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Teams
{
    [Table("TeamLinks")]
    public class TeamLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public Guid RaceId { get; set; }

        public TeamLink()
        {
        }
    }
}
