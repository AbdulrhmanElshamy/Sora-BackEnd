using Sofra.Api.Models;

namespace Sofra.Api.Contracts.Meal
{
    public record MealRequest(
        string Name, string Description ,decimal Price,int PreparationTimeInMinute,IEnumerable<IFormFile> MealPhotos
        );

}
