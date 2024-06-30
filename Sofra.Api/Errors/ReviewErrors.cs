using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class ReviewErrors
{
    public static readonly Error ReviewNotFound =
        new("Review.NotFound", "No Review was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedReview =
    new("Review.Duplicated", "Another Review with the same Kitchen is already exists", StatusCodes.Status409Conflict);
}