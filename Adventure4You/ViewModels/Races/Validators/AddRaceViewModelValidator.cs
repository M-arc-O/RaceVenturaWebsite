using FluentValidation;

namespace Adventure4You.ViewModels.Races.Validations
{
    public class AddRaceViewModelValidator : AbstractValidator<AddRaceViewModel>
    {
        public AddRaceViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}
