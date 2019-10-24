using Adventure4YouAPI.ViewModels.Points;
using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class StageDetailViewModel
    {
        public Guid StageId { get; set; }
        public Guid RaceId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int? MimimumPointsToCompleteStage { get; set; }
        public List<PointDetailViewModel> Points { get; set; }
    }
}
