using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Cart;
using Sofra.Api.Data;
using Sofra.Api.Models;
using System.Security.Claims;
using Sofra.Api.Errors;
using Sofra.Api.Models;

namespace Sofra.Api.Services.CartServices
{
    public class CartService(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor) : ICartService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<Result<CartResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
           var CustomerId = await GetCustomerId(cancellationToken);

            var Cart = await _dbContext
                             .Carts
                             .Where(c => c.CustomerId == CustomerId && c.Id == id)
                             .Include(c => c.Items)
                             .FirstOrDefaultAsync(cancellationToken);

            if (Cart is null)
                return Result.Failure<CartResponse>(CartErrors.CartNotFound);

            return Result.Success(Cart.Adapt<CartResponse>());
        }


        public async Task<Result<CartResponse>> AddAsync(CartRequest request, CancellationToken cancellationToken = default)
        {
           
            var CurrentCustomerId = await GetCustomerId();

            var Cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.CustomerId == CurrentCustomerId, cancellationToken);

            if(Cart is null)
            {
                Cart = new Cart()
                {
                    CustomerId = CurrentCustomerId,
                };

            }

            else
            {
                Cart.Items.Clear();
            }

            foreach (var item in request.Items) 
            {
                if (await _dbContext.Meals.AnyAsync(m => m.Id == item.MealId, cancellationToken))
                    return Result.Failure<CartResponse>(MealErrors.MealNotFound);
            }

            Cart.Items = request.Items.Adapt<ICollection<Models.CartItem>>();

             _dbContext.Update(Cart);
            await _dbContext.SaveChangesAsync();

            return Result.Success(Cart.Adapt<CartResponse>());
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
