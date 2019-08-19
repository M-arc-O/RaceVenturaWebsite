using System;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class StageDetailViewModel
    {
        public Guid RaceId { get; set; }
        public string Name { get; set; }
        public int? MimimumPointsToCompleteStage { get; set; }
    }
}
