using System;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class StageViewModel
    {
        public Guid Id { get; set; }

        public Guid RaceId { get; set; }

        public string Name { get; set; }

        public StageViewModel()
        {
        }
    }
}
