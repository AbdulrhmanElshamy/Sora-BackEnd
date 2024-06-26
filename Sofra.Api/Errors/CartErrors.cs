using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class CartErrors
{
    public static readonly Error CartNotFound =
        new("Cart.NotFound", "No Cart was found with the given ID", StatusCodes.Status404NotFound);
}