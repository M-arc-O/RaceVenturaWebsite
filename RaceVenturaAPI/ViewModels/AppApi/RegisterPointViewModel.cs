using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.AppApi
{
    public class RegisterPointViewModel
    {
        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [Required]
        public Guid UniqueId { get; set; }

        [RequiredNotEmpty]
        public Guid PointId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public PointTypeViewModel Type { get; set; }

        public string Message { get; set; }

        public string Answer { get; set; }
    }
}
