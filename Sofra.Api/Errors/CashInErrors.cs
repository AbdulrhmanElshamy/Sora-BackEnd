using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class CashInErrors
{
    public static readonly Error PaymentMethodNotFound =
        new("PaymentMethod.NotFound", "No Payment Method was found with the given ID", StatusCodes.Status404NotFound);
}