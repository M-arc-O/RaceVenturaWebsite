using Adventure4YouAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class RaceViewModel
    {
        public Guid RaceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
