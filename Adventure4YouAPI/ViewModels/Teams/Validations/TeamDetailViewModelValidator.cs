using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4YouAPI.ViewModels.Teams.Validations
{
    public class TeamDetailViewModelValidator: AbstractValidator<TeamDetailViewModel>
    {
        public TeamDetailViewModelValidator()
        {
            RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(vm => vm.RaceId).NotEmpty().WithMessage("RaceId cannot be empty");
        }
    }
}
