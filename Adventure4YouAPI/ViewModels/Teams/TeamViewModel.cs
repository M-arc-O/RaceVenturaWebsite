using System;

namespace Adventure4YouAPI.ViewModels.Teams
{
    public class TeamViewModel
    {
        public Guid Id { get; set; }
        public Guid RaceId { get; set; }
        public string Name { get; set; }
    }
}
