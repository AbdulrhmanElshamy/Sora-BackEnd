using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Order;
using Sofra.Api.Data;
using System.Security.Claims;

namespace Sofra.Api.Services.OrderServices
{
    public class OrderService(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor) : IOrderService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<OrderResponse>> AddAsync(OrderRequest request, CancellationToken cancellationToken = default)
        {

            throw new NotImplementedException();


        }

        public Task<Result> DeleteAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
