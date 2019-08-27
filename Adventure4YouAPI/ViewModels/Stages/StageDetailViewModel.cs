using System;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class StageDetailViewModel : StageViewModel
    {
        public Guid RaceId { get; set; }
        public int? MimimumPointsToCompleteStage { get; set; }
    }
}
