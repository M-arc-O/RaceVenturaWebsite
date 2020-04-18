using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class StageViewModel
    {
        public Guid StageId { get; set; }
        public Guid RaceId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int? MimimumPointsToCompleteStage { get; set; }
        public List<PointViewModel> Points { get; set; }
    }
}
