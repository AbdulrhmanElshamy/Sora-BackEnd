using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.CashIn;
using Sofra.Api.Contracts.paymob;
using Sofra.Api.Hubs;
using Sofra.Api.Models;
using Sofra.Api.Services.CashInServices;
using Sofra.Api.Services.KitchenServices;
using Sofra.Api.Services.OrderServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashInController(ICashInService cashInService,IOrderService orderService,IHubContext<NotificationHub> hubContext,IKitchenService kitchenService) : ControllerBase
    {
        private readonly ICashInService _cashInService = cashInService;
        private readonly IOrderService _orderService = orderService;
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;
        private readonly IKitchenService _kitchenService = kitchenService;

        [HttpPost("checkout/{orderId}")]
        public async Task<IActionResult> Checkout([FromRoute] int orderId,CashInRequest request,CancellationToken cancellationToken = default!)
        {
            var result = await _orderService.ConfirmAsync(int.Parse("2"));

            await _hubContext.Clients.Client(result.Value.KitchenId.ToString()).SendAsync("ReceiveNotification", $"تم طلب أوردر جديد لمطبخ {result.Value}!");


            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback([FromQuery] string txn_response_code, [FromQuery] int captured_amount,
           [FromQuery] string updated_at, [FromQuery] bool is_auth, [FromQuery] bool is_void, [FromQuery] bool is_refunded,
           [FromQuery] bool has_parent_transaction, [FromQuery] bool is_standalone_payment, [FromQuery] string merchant_order_id,
           [FromQuery] string id, [FromQuery] string acq_response_code, [FromQuery] decimal amount_cents, [FromQuery] bool is_voided,
           [FromQuery] CallbackDataRequest data, [FromQuery] decimal refunded_amount_cents,
           [FromQuery] CallbackSourceDataRequest source_data, [FromQuery] bool error_occured, [FromQuery] bool is_capture,
           [FromQuery] string currency, [FromQuery] bool pending, [FromQuery] string order, [FromQuery] string hmac,
           [FromQuery] string profile_id, [FromQuery] string integration_id, [FromQuery] bool is_refund, [FromQuery] string created_at,
           [FromQuery] bool success, [FromQuery] float merchant_commission, [FromQuery] bool is_3d_secure, [FromQuery] string owner)
        {
            //get HMAC
            string calculated_hmac = $"{amount_cents}{created_at}{currency}" +
                $"{error_occured.ToString().ToLower()}{has_parent_transaction.ToString().ToLower()}" +
                $"{id}{integration_id}{is_3d_secure.ToString().ToLower()}" +
                $"{is_auth.ToString().ToLower()}{is_capture.ToString().ToLower()}" +
                $"{is_refunded.ToString().ToLower()}{is_standalone_payment.ToString().ToLower()}" +
                $"{is_voided.ToString().ToLower()}{order}{owner}" +
                $"{pending.ToString().ToLower()}{source_data.pan}{source_data.sub_type}" +
                $"{source_data.type}{success.ToString().ToLower()}";



            string hmacKey = "50DE1FC724B86E2B1DDD6F94DD7CA71F";
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(hmacKey)))
            {
                var hashBytes = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(calculated_hmac));
                calculated_hmac = BitConverter.ToString(hashBytes).Replace("-", "");
            }
            if (success)
            {
                var result = await _orderService.ConfirmAsync(int.Parse(order));

                if(result.IsSuccess)
                {
                    await _hubContext.Clients.Client(result.Value.KitchenId.ToString()).SendAsync("ReceiveNotification", $"تم طلب أوردر جديد لمطبخ {result.Value}!");
                    return Ok();
                }

                return result.ToProblem();
            }


            return BadRequest();
        }
    }
}
