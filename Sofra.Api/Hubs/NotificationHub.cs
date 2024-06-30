using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Polly;
using System.Text.RegularExpressions;

namespace Sofra.Api.Hubs
{
    [Authorize(Roles = "Kitchen")]
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            try
            {

                Context.Items.Add(Context.UserIdentifier, Context.ConnectionId);
                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Context.Items.Remove(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}

