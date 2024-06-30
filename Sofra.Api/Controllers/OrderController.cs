using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Order;
using Sofra.Api.Services.OrderServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };


        [HttpGet("")]
        [Authorize(Roles = "Kitchen")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _orderService.GetAllAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromBody] OrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _orderService.AddAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Clear([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _orderService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

    }
}
