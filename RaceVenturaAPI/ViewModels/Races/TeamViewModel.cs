using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Races
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

        [Required]
        public TeamCategoryViewModel Category { get; set; }

        public DateTime? FinishTime { get; set; }

        public byte[] QrCodeArray { get; set; }

        public List<VisitedPointViewModel> VisitedPoints { get; set; }

        public List<FinishedStageViewModel> FinishedStages { get; set; }
    }
}
