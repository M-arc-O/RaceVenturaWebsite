using Adventure4YouAPI.ViewModels.Validators;
using System;

namespace Adventure4YouAPI.ViewModels.AppApi
{
    public class RegisterToRaceViewModel
    {
        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        [RequiredNotEmpty]
        public Guid UniqueId { get; set; }
    }
}
