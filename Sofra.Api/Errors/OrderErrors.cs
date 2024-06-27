using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class OrderErrors
{
    public static readonly Error OrderNotFound =
       new("Order.NotFound", "No Order was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error AllItemsFromOneKitchen =
        new("Order.DifferentKitchens", "All Items must be from one kitchen", StatusCodes.Status400BadRequest);
}