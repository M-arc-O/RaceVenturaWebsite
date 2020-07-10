using RaceVenturaAPI.ViewModels.Validators;
using System;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class FinishedStageViewModel
    {
        public Guid FinishedStageId { get; set; }

        [RequiredNotEmpty]
        public Guid StageId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        public DateTime FinishTime { get; set; }
    }
}
