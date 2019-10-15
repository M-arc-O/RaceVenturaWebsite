using System;

namespace Adventure4YouAPI.ViewModels.TeamRace
{
    public class RegisterPointViewModel
    {
        public Guid RaceId { get; set; }
        public Guid StageId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PointId { get; set; }
    }
}
