using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.AppApi
{
    public class RegisterStageEndViewModel
    {
        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [Required]
        public Guid UniqueId { get; set; }

        [RequiredNotEmpty]
        public Guid StageId { get; set; }
    }
}
