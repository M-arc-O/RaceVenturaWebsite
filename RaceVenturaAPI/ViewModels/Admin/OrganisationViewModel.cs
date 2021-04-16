using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Admin
{
    public class OrganisationViewModel
    {
        public Guid OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
