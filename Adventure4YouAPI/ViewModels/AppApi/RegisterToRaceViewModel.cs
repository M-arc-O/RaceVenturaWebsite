using Adventure4YouAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPI.ViewModels.AppApi
{
    public class RegisterToRaceViewModel
    {
        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        [Required]
        public string UniqueId { get; set; }
    }
}
