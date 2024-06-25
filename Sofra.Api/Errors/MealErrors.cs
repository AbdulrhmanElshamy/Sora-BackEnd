using Sofra.Api.Abstractions;

namespace UniLearn.API.Errors;

public static class MealErrors
{
    public static readonly Error MealNotFound =
        new("Meal.NotFound", "No Meal was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedMealTitle =
        new("Meal.DuplicatedTitle", "Another Meal with the same title is already exists", StatusCodes.Status409Conflict);
}