using System;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class VisitedPointViewModel
    {
        public Guid VisitedPointId { get; set; }
        public Guid PointId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime Time { get; set; }
    }
}
