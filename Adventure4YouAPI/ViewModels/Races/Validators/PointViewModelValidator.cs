using FluentValidation;

namespace Adventure4YouAPI.ViewModels.Races.Validators
{
    public class PointViewModelValidator : AbstractValidator<PointViewModel>
    {
        public PointViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(vm => vm.Value).GreaterThan(0).WithMessage("Value must be greater than 0");
            RuleFor(vm => vm.StageId).NotEmpty().WithMessage("Stage id cannot be empty");
        }
    }
}
