using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Review;

namespace Sofra.Api.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<Result<List<ReviewResponse>>> GetAllAsync(int KitchenId, CancellationToken cancellationToken = default!);

        Task<Result<ReviewResponse>> GetAsync(int id, CancellationToken cancellationToken = default!);

        Task<Result<ReviewResponse>> AddAsync(ReviewRequest request, CancellationToken cancellationToken = default!);

        Task<Result> UpdateAsync(int id,ReviewRequest request, CancellationToken cancellationToken = default!);
    }
}
