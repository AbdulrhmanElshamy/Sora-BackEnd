using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class CategoryErrors
{
    public static readonly Error CategoryNotFound =
        new("Category.NotFound", "No Category was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedCategoryTitle =
        new("Category.DuplicatedTitle", "Another Category with the same title is already exists", StatusCodes.Status409Conflict);
}