using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Review;
using Sofra.Api.Data;
using Sofra.Api.Errors;
using Sofra.Api.Models;
using System.Security.Claims;

namespace Sofra.Api.Services.ReviewServices
{
    public class ReviewService(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor) : IReviewService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<ReviewResponse>> AddAsync(ReviewRequest request, CancellationToken cancellationToken = default!)
        {
            var CurrentCustomerId = await GetCustomerId();

            var CustomerOrderFormKitchen = await _dbContext.Orders.AnyAsync(
                o => o.CustomerId == CurrentCustomerId && o.OrderDetails.Select(d => d.Meal.KitchenId).FirstOrDefault() == request.kitchenId, cancellationToken);

            if (!CustomerOrderFormKitchen)
                return Result.Failure<ReviewResponse>(OrderErrors.OrderNotFound);

            var CustomerHasReview = await _dbContext.Reviews.AnyAsync(r => r.CustomerId == CurrentCustomerId, cancellationToken);

            if(CustomerHasReview)
                return Result.Failure<ReviewResponse>(ReviewErrors.DuplicatedReview);

            var review = request.Adapt<Review>();

            await _dbContext.AddAsync(review,cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(review.Adapt<ReviewResponse>());

        }

        public async Task<Result<List<ReviewResponse>>> GetAllAsync(int KitchenId,CancellationToken cancellationToken = default !)
        {
            var reviews = await _dbContext.Reviews.Where(r => r.KitchenId == KitchenId).ToListAsync(cancellationToken);

            return Result.Success(reviews.Adapt<List<ReviewResponse>>());

        }

        public async Task<Result<ReviewResponse>> GetAsync(int id, CancellationToken cancellationToken = default!)
        {
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);  

            if(review is null)
                return Result.Failure<ReviewResponse>(ReviewErrors.ReviewNotFound);

            return Result.Success(review.Adapt<ReviewResponse>());
        }

        public async Task<Result> UpdateAsync(int id, ReviewRequest request, CancellationToken cancellationToken = default!)
        {
            var CurrentCustomerId = await GetCustomerId();

            var CustomerOrderFormKitchen = await _dbContext.Orders.AnyAsync(
                o => o.CustomerId == CurrentCustomerId && o.OrderDetails.Select(d => d.Meal.KitchenId).FirstOrDefault() == request.kitchenId, cancellationToken);

            if (!CustomerOrderFormKitchen)
                return Result.Failure<ReviewResponse>(OrderErrors.OrderNotFound);

            var CustomerHasReview = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.CustomerId == CurrentCustomerId, cancellationToken);

            if (CustomerHasReview is null)
                return Result.Failure<ReviewResponse>(ReviewErrors.DuplicatedReview);

            CustomerHasReview.Rate = request.Rate;
            CustomerHasReview.Comment = request.Comment;

            _dbContext.Update(CustomerHasReview);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        private async Task<int> GetCustomerId(CancellationToken cancellationToken = default!)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentCustomerId = await _dbContext
                                          .Customers
                                          .Where(c => c.ApplicationUserId == CurrentUserId)
                                          .Select(c => c.Id)
                                          .FirstOrDefaultAsync(cancellationToken);
            return CurrentCustomerId;
        }
    }
}
