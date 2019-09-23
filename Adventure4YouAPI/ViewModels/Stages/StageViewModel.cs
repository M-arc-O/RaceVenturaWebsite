using System;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class StageViewModel
    {
        public Guid StageId { get; set; }

        public Guid RaceId { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public StageViewModel()
        {
        }
    }
}
