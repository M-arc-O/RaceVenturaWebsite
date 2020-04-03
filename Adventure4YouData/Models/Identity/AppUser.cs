using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouData.Models.Identity
{
    public class AppUser: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
