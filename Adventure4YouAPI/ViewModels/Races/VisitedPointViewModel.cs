using Adventure4YouAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class VisitedPointViewModel
    {
        public Guid VisitedPointId { get; set; }

        [RequiredNotEmpty]
        public Guid PointId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}
