using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class RaceViewModel
    {
        public Guid RaceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
