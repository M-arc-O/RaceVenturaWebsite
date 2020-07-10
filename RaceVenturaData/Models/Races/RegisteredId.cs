using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceVenturaData.Models.Races
{
    public class RegisteredId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RegisteredIdId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UniqueId { get; set; }
    }
}
