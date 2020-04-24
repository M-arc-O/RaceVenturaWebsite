using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class FinishedStageViewModel
    {
        public Guid FinishedStageId { get; set; }
        public Guid StageId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
