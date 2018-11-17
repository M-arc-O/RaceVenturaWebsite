using FluentValidation;

namespace Adventure4You.ViewModels.Races.Validations
{
    public class AddPointViewModelValidator : AbstractValidator<AddRaceViewModel>
    {
        public AddPointViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.CoordinatesCheckEnabled).NotEmpty().WithMessage("Coordinates check enabled cannot be empty");
            RuleFor(vm => vm.SpecialTasksAreStage).NotEmpty().WithMessage("Special tasks are stage cannot be empty");
        }
    }
}
