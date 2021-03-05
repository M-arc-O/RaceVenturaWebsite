using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class RaceAccessViewModel
    {
        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public AccessLevelViewModel AccessLevel { get; set; }
    }
}
