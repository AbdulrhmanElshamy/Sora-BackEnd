using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Category;
using Sofra.Api.Services.NotificationSrevices;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Kitchen")]
    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _notificationService.GetNotificationsAsync(cancellationToken));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,
        CancellationToken cancellationToken)
        {
            var result = await _notificationService.UpdateStatusAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
