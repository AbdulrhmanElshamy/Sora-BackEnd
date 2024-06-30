using Sofra.Api.Abstractions;
using Sofra.Api.Models;

namespace Sofra.Api.Services.NotificationSrevices
{
    public interface INotificationService
    {
        Task<Result<List<Notification>>> GetNotificationsAsync(CancellationToken cancellationToken = default!);

        Task<Result> AddAsync(Notification notification, CancellationToken cancellationToken = default!);

        Task<Result> UpdateStatusAsync(int Id, CancellationToken cancellationToken = default!);
    }
}
