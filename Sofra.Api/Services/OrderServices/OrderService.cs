using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Order;
using Sofra.Api.Data;
using Sofra.Api.Errors;
using Sofra.Api.Enums;
using System.Security.Claims;
using Sofra.Api.Models;

namespace Sofra.Api.Services.OrderServices
{
    public class OrderService(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor) : IOrderService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<Result<OrderResponse>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {

            var order = await Get(Id, cancellationToken);

            if (order is null)
                return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);
        
            return Result.Success(order.Adapt<OrderResponse>());
        }



        public async Task<Result<OrderResponse>> AddAsync(OrderRequest request, CancellationToken cancellationToken = default)
        {

            var Kitchen = await _dbContext.Meals.FirstOrDefaultAsync(k => k.Id == request.Details.Select(m => m.Id).FirstOrDefault(),cancellationToken);
            if (Kitchen is null)
                return Result.Failure<OrderResponse>(MealErrors.MealNotFound);


            var AllItemsFromOneKitchen = await _dbContext.Meals.AllAsync(c => c.KitchenId == Kitchen.Id);

            if (!AllItemsFromOneKitchen)
                return Result.Failure<OrderResponse>(OrderErrors.AllItemsFromOneKitchen);

            var CurrentCustomerId = await GetCustomerId();

            var order = await _dbContext.Orders.FirstOrDefaultAsync(c => c.CustomerId == CurrentCustomerId && c.Status == OrderStatus.Pending, cancellationToken);

            if (order is null)
            {
                order = new Order()
                {
                    CustomerId = CurrentCustomerId,
                };

            }

            else
            {
                order.OrderDetails.Clear();
            }

            foreach (var item in request.Details)
            {
                if (await _dbContext.Meals.AnyAsync(m => m.Id == item.MealId, cancellationToken))
                    return Result.Failure<OrderResponse>(MealErrors.MealNotFound);


            }

            order.OrderDetails = request.Details.Adapt<ICollection<Models.OrderDetail>>();

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();

            return Result.Success(order.Adapt<OrderResponse>());


        }



        public async Task<Result> ConfirmAsync(int Id,CancellationToken cancellationToken = default)
        {
            var order = await Get(Id, cancellationToken);


            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);


        }



        public async Task<Result> DeleteAsync(int Id, CancellationToken cancellationToken = default)
        {

            var order = await Get(Id, cancellationToken);


            if (order is null) 
                return Result.Failure(OrderErrors.OrderNotFound);


            _dbContext.Remove(order);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return Result.Success();
        }


        private async Task<Order?> Get(int Id,CancellationToken cancellationToken = default!)
        {
            var CurrentCustomerId = await GetCustomerId();

            var order = await _dbContext
                              .Orders
                              .FirstOrDefaultAsync(
                               c => c.CustomerId == CurrentCustomerId &&
                               c.Status == OrderStatus.Pending &&
                               c.Id == Id &&
                               c.CustomerId == CurrentCustomerId, cancellationToken);

            return order;
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
