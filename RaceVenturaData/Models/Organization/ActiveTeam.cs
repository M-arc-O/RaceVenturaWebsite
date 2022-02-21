using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceVenturaData.Models.Organization
{
    public class ActiveTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ActiveTeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime ActivationDate { get; set; }
    }
}
