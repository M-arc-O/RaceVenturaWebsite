using FluentValidation;

namespace Adventure4YouAPI.ViewModels.Stages.Validations
{
    public class StageViewModelValidator : AbstractValidator<StageViewModel>
    {
        public StageViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.RaceId).NotEmpty().WithMessage("RaceId cannot be empty");
        }
    }
}
