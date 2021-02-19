
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Identity
{
    public class CredentialsViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
