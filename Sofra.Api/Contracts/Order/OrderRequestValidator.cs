using FluentValidation;
using Sofra.Api.Contracts.Order;

namespace Sofra.Api.Contracts.Meal
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.Details)
                .NotEmpty();
        }
    }
}
