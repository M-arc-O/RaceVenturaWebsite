using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Admin
{
    public class OrganizationViewModel
    {
        public Guid OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
