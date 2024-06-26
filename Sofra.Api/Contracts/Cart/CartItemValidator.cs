using FluentValidation;
using Sofra.Api.Contracts.Cart;

namespace Sofra.Api.Contracts.Meal
{
    public class CartItemValidator : AbstractValidator<CartItem>
    {
        public CartItemValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
