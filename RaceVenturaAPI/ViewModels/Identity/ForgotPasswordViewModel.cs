using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Identity
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
