using FluentValidation;

namespace Adventure4YouAPI.ViewModels.Races.Validators
{
    public class TeamViewModelValidator: AbstractValidator<TeamViewModel>
    {
        public TeamViewModelValidator()
        {
            RuleFor(vm => vm.RaceId).NotEmpty().WithMessage("RaceId cannot be empty");
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.Number).NotEmpty().WithMessage("Number cannot be empty");
            RuleFor(vm => vm.Number).GreaterThanOrEqualTo(1).WithMessage("Number must be greater than or equal to 1");
        }
    }
}
