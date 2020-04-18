using System;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class VisitedPointViewModel
    {
        public Guid TeamPointVisitedId { get; set; }
        public Guid RaceId { get; set; }
        public Guid StageId { get; set; }
        public Guid PointId { get; set; }
        public Guid TeamId { get; set; }
    }
}
