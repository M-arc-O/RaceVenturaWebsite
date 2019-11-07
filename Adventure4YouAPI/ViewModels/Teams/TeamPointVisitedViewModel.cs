using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4YouAPI.ViewModels.Teams
{
    public class TeamPointVisitedViewModel
    {
        public Guid TeamPointVisitedId { get; set; }
        public Guid RaceId { get; set; }
        public Guid StageId { get; set; }
        public Guid PointId { get; set; }
        public Guid TeamId { get; set; }
    }
}
