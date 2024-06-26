using FluentValidation;
using Sofra.Api.Contracts.Order;

namespace Sofra.Api.Contracts.Meal
{
    public class OrderDetailValidator : AbstractValidator<OrderDetail>
    {
        public OrderDetailValidator()
        {
            RuleFor(x => x.MealId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Quantity)
    .NotEmpty()
    .GreaterThan(0);
        }
    }
}
