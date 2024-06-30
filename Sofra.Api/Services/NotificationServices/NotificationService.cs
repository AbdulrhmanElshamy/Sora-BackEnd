using Microsoft.EntityFrameworkCore;
using Sofra.Api.Abstractions;
using Sofra.Api.Data;
using Sofra.Api.Errors;
using Sofra.Api.Models;
using System.Security.Claims;

namespace Sofra.Api.Services.NotificationSrevices
{
    public class NotificationService(ApplicationDbContext dbContext , IHttpContextAccessor httpContextAccessor) : INotificationService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> AddAsync(Notification notification,CancellationToken cancellationToken = default!)
        {
            await _dbContext.AddAsync(notification,cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<List<Notification>>> GetNotificationsAsync(CancellationToken cancellationToken = default)
        {
            var CurrentKitchenId = await GetKitchenId();

            var notifications = await _dbContext
                        .Notifications
                        .AsNoTracking()
                        .Where(n => !n.IsRead && n.KitchenId == CurrentKitchenId)
                        .ToListAsync(cancellationToken);

            return Result.Success(notifications);
        }

        public async Task<Result> UpdateStatusAsync(int Id, CancellationToken cancellationToken = default!)
        {
            var CurrentKitchenId = await GetKitchenId();

            var notification = await _dbContext
                        .Notifications
                        .AsNoTracking()
                        .FirstOrDefaultAsync(n => !n.IsRead && n.Id == Id && n.KitchenId == CurrentKitchenId);


            if(notification is null)
                return Result.Failure(NotificationErrors.NotificationNotFound);

            _dbContext.Remove(notification);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }



        private async Task<int> GetKitchenId(CancellationToken cancellationToken = default!)
        {
            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var CurrentKitchenId = await _dbContext
                                          .Kitchens
                                          .Where(c => c.ApplicationUserId == CurrentUserId)
                                          .Select(c => c.Id)
                                          .FirstOrDefaultAsync(cancellationToken);
            return CurrentKitchenId;
        }
    }
}
