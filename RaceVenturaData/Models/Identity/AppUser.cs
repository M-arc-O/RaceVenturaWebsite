using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaData.Models.Identity
{
    public class AppUser: IdentityUser
    {
        public Guid OrganizationId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
