using Adventure4YouAPI.ViewModels.Points;
using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class StageDetailViewModel : StageViewModel
    {
        public int? MimimumPointsToCompleteStage { get; set; }
        public List<PointDetailViewModel> Points { get; set; }
    }
}
