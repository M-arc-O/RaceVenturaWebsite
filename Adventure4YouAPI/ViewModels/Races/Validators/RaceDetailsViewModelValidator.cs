using FluentValidation;

namespace Adventure4YouAPI.ViewModels.Races.Validators
{
    public class RaceDetailsViewModelValidator : AbstractValidator<RaceDetailViewModel>
    {
        public RaceDetailsViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.MaximumTeamSize).GreaterThan(0).WithMessage("MaximumTeamSize must be greater than zero");
            RuleFor(vm => vm.MinimumPointsToCompleteStage).GreaterThan(0).WithMessage("MinimumPointsToCompleteStage must be greater than zero");
            RuleFor(vm => vm.PenaltyPerMinuteLate).GreaterThanOrEqualTo(0).WithMessage("PenaltyPerMinutLate must be equal to or greater than zero");
            RuleFor(vm => vm.StartTime).NotEmpty().WithMessage("Start time cannot be empty");
            RuleFor(vm => vm.EndTime).NotEmpty().WithMessage("End time cannot be empty");
        }
    }
}
