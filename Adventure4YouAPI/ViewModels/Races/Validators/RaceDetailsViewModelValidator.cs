using FluentValidation;

namespace Adventure4YouAPI.ViewModels.Races.Validations
{
    public class RaceDetailsViewModelValidator : AbstractValidator<RaceDetailViewModel>
    {
        public RaceDetailsViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.MaximumTeamSize).GreaterThan(0).WithMessage("MaximumTeamSize must be greater than zero");
            RuleFor(vm => vm.MinimumPointsToCompleteStage).GreaterThan(0).WithMessage("MinimumPointsToCompleteStage must be greater than zero");
        }
    }
}
