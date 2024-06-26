using FluentValidation;

namespace Sofra.Api.Contracts.Meal
{
    public class CartRequestValidator : AbstractValidator<MealRequest>
    {
        public CartRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
            
            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(20)
                .MaximumLength(250);
            
            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(10);

                RuleFor(x => x.PreparationTimeInMinute)
                .NotEmpty()
                .GreaterThan(10);

            RuleFor(x => x.MealPhotos)
                .NotEmpty();
        }
    }
}
