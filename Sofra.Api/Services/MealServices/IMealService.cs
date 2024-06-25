using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Meal;

namespace Sofra.Api.Services.MealServices
{
    public interface IMealService
    {
        Task<IEnumerable<MealResponse>> GetAllAsync(int? KitchenId,string? SearchText, CancellationToken cancellationToken = default);

        Task<Result<MealResponse>> GetAsync(int Id, CancellationToken cancellationToken = default);
        
        Task<Result<MealResponse>> AddAsync(MealRequest request, CancellationToken cancellationToken = default);
        
        Task<Result> UpdateAsync(int Id, MealRequest request, CancellationToken cancellationToken = default);

        Task<Result> ToggleStatus(int Id, CancellationToken cancellationToken = default);

        Task<Result> DeletedAsync(int Id, CancellationToken cancellationToken = default);
    }
}
