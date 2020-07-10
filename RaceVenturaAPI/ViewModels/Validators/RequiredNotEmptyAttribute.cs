using System;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPI.ViewModels.Validators
{
    [AttributeUsage(
       AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
       AllowMultiple = false)]
    public class RequiredNotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public RequiredNotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            return value switch
            {

                Guid guid => guid != Guid.Empty,
                _ => true,
            };
        }
    }
}
