using Adventure4YouAPI.ViewModels.Validators;
using System;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class FinishedStageViewModel
    {
        [RequiredNotEmpty]
        public Guid FinishedStageId { get; set; }

        [RequiredNotEmpty]
        public Guid StageId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        public DateTime FinishTime { get; set; }
    }
}
