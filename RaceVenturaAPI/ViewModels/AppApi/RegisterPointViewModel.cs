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
        public string UniqueId { get; set; }

        [RequiredNotEmpty]
        public Guid PointId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
