using RaceVenturaAPI.ViewModels.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Admin
{
    public class AddUserToOrganizationViewModel
    {
        [RequiredNotEmpty]
        public Guid OrganizationId { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
