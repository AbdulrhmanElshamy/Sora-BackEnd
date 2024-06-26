using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class KitchenErrors
{
    public static readonly Error KitchenNotFound =
        new("Kitchen.NotFound", "No Kitchen was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedKitchenTitle =
        new("Kitchen.DuplicatedTitle", "Another Kitchen with the same title is already exists", StatusCodes.Status409Conflict);
}