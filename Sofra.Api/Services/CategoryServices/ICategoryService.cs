using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Category;

namespace Sofra.Api.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default!);

        Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default!);

        Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default!);

        Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default);

        Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);
    }
}
