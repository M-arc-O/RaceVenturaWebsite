
using System;

namespace Adventure4YouAPI.ViewModels.Teams
{
    public class TeamDetailViewModel
    {
        public Guid TeamId { get; set; }
        public Guid RaceId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
