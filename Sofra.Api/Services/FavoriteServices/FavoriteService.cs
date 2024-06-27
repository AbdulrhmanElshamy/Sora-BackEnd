using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Cart;
using Sofra.Api.Contracts.Favorite;
using Sofra.Api.Data;
using Sofra.Api.Errors;
using System.Security.Claims;
using Sofra.Api.Models;
namespace Sofra.Api.Services.FavoriteServices
{
    public class FavoriteService(ApplicationDbContext _dbContext, IHttpContextAccessor _httpContextAccessor) : IFavoriteService
    {
        public async Task<Result<FavoriteResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var CustomerId = await GetCustomerId(cancellationToken);

            var Favorite = await _dbContext
                             .Favorites
                             .Where(c => c.CustomerId == CustomerId && c.Id == id)
                             .Include(c => c.Items)
                             .FirstOrDefaultAsync(cancellationToken);

            if (Favorite is null)
                return Result.Failure<FavoriteResponse>(FavoriteErrors.FavoriteNotFound);

            return Result.Success(Favorite.Adapt<FavoriteResponse>());
        }
        public async Task<Result<FavoriteResponse>> AddAsync(FavoriteRequest request, CancellationToken cancellationToken = default)
        {
            var CurrentCustomerId = await GetCustomerId();

            var Favorite = await _dbContext.Favorites.FirstOrDefaultAsync(c => c.CustomerId == CurrentCustomerId, cancellationToken);

            if (Favorite is null)
            {
                Favorite = new Favorite()
                {
                    CustomerId = CurrentCustomerId,
                };

            }

            else
            {
                Favorite.Items.Clear();
            }

            Favorite.Items = request.Items.Adapt<ICollection<Models.FavoriteItem>>();

            _dbContext.Update(Favorite);
            await _dbContext.SaveChangesAsync();

            return Result.Success(Favorite.Adapt<FavoriteResponse>());
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
