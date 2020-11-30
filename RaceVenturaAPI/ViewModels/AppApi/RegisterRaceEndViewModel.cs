using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.AppApi
{
    public class RegisterRaceEndViewModel
    {
        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [Required]
        public string UniqueId { get; set; }
    }
}
