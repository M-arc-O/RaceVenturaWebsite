using RaceVenturaData.Models.Races;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceVenturaData.Models.Organization
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EventId { get; set; }

        [Required]
        public Guid RaceId { get; set; }

        [Required]
        public string EventName { get; set; }

        public List<ActiveTeam> ActiveTeams { get; set; }
    }
}
