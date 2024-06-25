using FluentValidation;

namespace Sofra.Api.Contracts.Kitchen
{
    public class KitchenRegisterRequestValidator : AbstractValidator<KitchenRegisterRequest>
    {
        public KitchenRegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();


            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Avatar)
                .NotEmpty();

            RuleFor(x => x.Categories)
                .NotEmpty();

            RuleFor(x => x.MaxDeliveryDistance)
                .NotEmpty()
                .GreaterThan(1)
                .LessThan(10);
        }
    }
}
