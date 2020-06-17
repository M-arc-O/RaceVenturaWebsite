using Adventure4YouAPI.ViewModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class TeamViewModel
    {
        public Guid TeamId { get; set; }

        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<VisitedPointViewModel> VisitedPoints { get; set; }

        public DateTime FinishTime { get; set; }

        public List<FinishedStageViewModel> FinishedStages { get; set; }
    }
}
