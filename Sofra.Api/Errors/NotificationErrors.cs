using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class NotificationErrors
{
    public static readonly Error NotificationNotFound =
        new("Notification.NotFound", "No Notification was found with the given ID", StatusCodes.Status404NotFound);
}