using FluentValidation;

namespace Sofra.Api.Contracts.Kitchen
{
    public class KitchenUpdateRequestValidator : AbstractValidator<KitchenUpdateRequest>
    {
        public KitchenUpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.Categories)
                .NotEmpty();

            RuleFor(x => x.MaxDeliveryDistance)
                .NotEmpty()
                .GreaterThan(1)
                .LessThan(10);
        }
    }
}
