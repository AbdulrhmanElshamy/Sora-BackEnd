using Mapster;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Order;
using Sofra.Api.Data;
using Sofra.Api.Errors;
using Sofra.Api.Enums;
using System.Security.Claims;
using Sofra.Api.Models;
using Azure.Core;
using Sofra.Api.Contracts.CashIn;
using Sofra.Api.Services.NotificationSrevices;

namespace Sofra.Api.Services.OrderServices
{
    public class OrderService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,INotificationService notificationService) : IOrderService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly INotificationService _notificationService = notificationService;



        public async Task<Result<List<OrderResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchenId = await _dbContext
                                          .Kitchens
                                          .Where(c => c.ApplicationUserId == CurrentUserId)
                                          .Select(c => c.Id)
                                          .FirstOrDefaultAsync(cancellationToken);

            var orders = await _dbContext.Orders.Where(o => o.KitchenId == CurrentKitchenId).ToListAsync(cancellationToken);

            return Result.Success(orders.Adapt<List<OrderResponse>>());
        }


        public async Task<Result<OrderResponse>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {

            var order = await Get(Id, cancellationToken);

            if (order is null)
                return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);

            return Result.Success(order.Adapt<OrderResponse>());
        }



        public async Task<Result<OrderResponse>> AddAsync(OrderRequest request, CancellationToken cancellationToken = default)
        {

            var meal = await _dbContext.Meals.FirstOrDefaultAsync(k => k.Id == request.Details.Select(m => m.MealId).FirstOrDefault(), cancellationToken);
            if (meal is null)
                return Result.Failure<OrderResponse>(MealErrors.MealNotFound);

            foreach (var item in request.Details)
            {
                if (!(await _dbContext.Meals.Include(m => m.Kitchen).AnyAsync(m => m.Id == item.MealId && m.KitchenId == meal.KitchenId)))
                    return Result.Failure<OrderResponse>(OrderErrors.AllItemsFromOneKitchen);

                if (!(await _dbContext.Kitchens.AnyAsync(k => k.Id == meal.KitchenId && k.Enabled, cancellationToken)))
                    return Result.Failure<OrderResponse>(KitchenErrors.KitchenClosed);

            }

            var CurrentCustomerId = await GetCustomerId();

            var order = await _dbContext.Orders.FirstOrDefaultAsync(c => c.CustomerId == CurrentCustomerId && c.Status == OrderStatus.Pending, cancellationToken);

            if (order is null)
            {
                order = new Order()
                {
                    CustomerId = CurrentCustomerId,
                    KitchenId = meal.KitchenId
                };

            }

            else
            {
                order.OrderDetails.Clear();
            }

            foreach (var item in request.Details)
            {
                if (!await _dbContext.Meals.AnyAsync(m => m.Id == item.MealId && m.IsAvailable, cancellationToken))
                    return Result.Failure<OrderResponse>(MealErrors.MealNotFound);


            }

            order.OrderDetails = request.Details.Adapt<ICollection<Models.OrderDetail>>();
            order.Payment = request.PaymentType;
            var mealPrices = await Task.WhenAll(request.Details.Select(async d =>
                                    new { d.Quantity, Price = await _dbContext.Meals.Where(m => m.Id == d.MealId).Select(m => m.Price).FirstOrDefaultAsync() }));

            order.TotalPrice = mealPrices.Sum(item => item.Quantity * item.Price);
            order.Notes = request.Notes;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();

            return Result.Success(order.Adapt<OrderResponse>());


        }



        public async Task<Result<Notification>> ConfirmAsync(int Id)
        {
            var order = await Get(Id);

            if (order is null)
                return Result.Failure<Notification>(OrderErrors.OrderNotFound);

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();

            var orderDetail = await _dbContext.OrderDetails.FirstOrDefaultAsync(k => k.OrderId == order.Id);
            if(orderDetail is null)
                                return Result.Failure<Notification>(NotificationErrors.NotificationNotFound);


            var KitchenId = await _dbContext.Meals.Where(m => m.Id == orderDetail.MealId).Include(m => m.Kitchen).Select(m => m.KitchenId).FirstOrDefaultAsync();

            var notification = new Notification()
            {
                Title = "طلب جديد",
                Description = $"عدد الواجبات المطلوبة{order.OrderDetails.Count()}",
                KitchenId = KitchenId
            };
            var result = _notificationService.AddAsync(notification);
            if (result.IsFaulted)
                return Result.Failure<Notification>(NotificationErrors.NotificationNotFound);

            return Result.Success(notification);
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


        private async Task<Order?> Get(int Id, CancellationToken cancellationToken = default!)
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
