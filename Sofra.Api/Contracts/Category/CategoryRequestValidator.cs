using FluentValidation;
using Sofra.Api.Contracts.Category;

namespace Sofra.Api.Contracts.Category
{
    public class AddressRequestValidator : AbstractValidator<CategoryRequest>
    {
        public AddressRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(25);
        }
    }
}
