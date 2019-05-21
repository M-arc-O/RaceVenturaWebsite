using Adventure4YouAPI.ViewModels.Identity.Validations;
using FluentValidation.Attributes;

namespace Adventure4YouAPI.ViewModels.Identity
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
