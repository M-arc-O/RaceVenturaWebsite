
using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Teams
{
    public class TeamDetailViewModel
    {
        public Guid TeamId { get; set; }
        public Guid RaceId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<TeamPointVisitedViewModel> PointsVisited { get; set; }
        public DateTime RaceFinished { get; set; }
    }
}
