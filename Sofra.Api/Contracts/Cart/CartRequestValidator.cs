using FluentValidation;

namespace Sofra.Api.Contracts.Cart
{
    public class CartRequestValidator : AbstractValidator<CartRequest>
    {
        public CartRequestValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty();
                
        }
    }
}
