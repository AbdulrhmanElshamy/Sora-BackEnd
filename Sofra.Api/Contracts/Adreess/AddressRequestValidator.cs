using FluentValidation;
using Sofra.Api.Contracts.Category;

namespace Sofra.Api.Contracts.Address
{
    public class AddressRequestValidator : AbstractValidator<AddressRequest>
    {
        public AddressRequestValidator()
        {
            RuleFor(x => x.Latitude)
                .NotEmpty();

            RuleFor(x => x.Longitude)
               .NotEmpty();

            RuleFor(location => location.Latitude)
             .Must(IsValidLatitude)
             .WithMessage("Latitude must be a valid number between 22.0 and 31.5 degrees.")
             .InclusiveBetween(22.0, 31.5)
             .WithMessage("Latitude must be between 22.0 and 31.5 degrees.");

            RuleFor(location => location.Longitude)
                .Must(IsValidLongitude)
                .WithMessage("Longitude must be a valid number between 25.0 and 35.0 degrees.")
                .InclusiveBetween(25.0, 35.0)
                .WithMessage("Longitude must be between 25.0 and 35.0 degrees.");
        }

        private bool IsValidLatitude(double latitude)
        {
            return latitude >= 22.0 && latitude <= 31.5;
        }

        private bool IsValidLongitude(double longitude)
        {
            return longitude >= 25.0 && longitude <= 35.0;
        }
    }
}
