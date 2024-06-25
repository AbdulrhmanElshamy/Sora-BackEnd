using Sofra.Api.Contracts.Category;

namespace Sofra.Api.Contracts.Meal
{
    public record MealResponse(
        int id, string Name, string Description, decimal Price, int PreparationTimeInMinute,bool IsAvailable ,List<string> MealPhotos
        );
}
