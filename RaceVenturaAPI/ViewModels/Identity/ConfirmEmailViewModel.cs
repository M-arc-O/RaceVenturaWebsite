using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Identity
{
    public class ConfirmEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        public string Code { get; set; }
    }
}
