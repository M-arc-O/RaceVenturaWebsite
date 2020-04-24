using Adventure4YouAPI.ViewModels.Validators;
using System;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class VisitedPointViewModel
    {
        [RequiredNotEmpty]
        public Guid VisitedPointId { get; set; }

        [RequiredNotEmpty]
        public Guid PointId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        public DateTime Time { get; set; }
    }
}
