using FluentValidation;
using Sofra.Api.Contracts.Customer;

namespace Sofra.Api.Contracts.Customer
{
    public class CustomerRegisterRequestValidator : AbstractValidator<CustomerRegisterRequest>
    {
        public CustomerRegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
            
            RuleFor(x => x.FristName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(15);
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(15);
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}
