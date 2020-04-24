using Adventure4YouAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class PointViewModel
    {
        public Guid PointId { get; set; }

        [RequiredNotEmpty]
        public Guid StageId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public PointTypeViewModel Type { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string Answer { get; set; }

        public string Message { get; set; }
    }
}
