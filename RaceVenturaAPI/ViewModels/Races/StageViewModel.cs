using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class StageViewModel
    {
        public Guid StageId { get; set; }

        [RequiredNotEmpty]
        public Guid RaceId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? MimimumPointsToCompleteStage { get; set; }

        public byte[] QrCodeArray { get; set; }

        public List<PointViewModel> Points { get; set; }
    }
}
