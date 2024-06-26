using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Address;
using Sofra.Api.Contracts.Kitchen;
using Sofra.Api.Services.KitchenServices;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Kitchen")]
    public class KitchenController(IKitchenService kitchenService) : ControllerBase
    {
        private readonly IKitchenService _kitchenService = kitchenService;

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(AddressRequest request, CancellationToken cancellationToken)
        {
            var result = await _kitchenService.GetAllAsync(request,cancellationToken);

            return Ok(result);
        }


        [HttpPost("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] int id,AddressRequest request ,CancellationToken cancellationToken)
        {
            var result = await _kitchenService.GetAsync(id,request,cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _kitchenService.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _kitchenService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("SetAddress")]
        public async Task<IActionResult> SetAddress(AddressRequest request, CancellationToken cancellationToken)
        {
            var result = await _kitchenService.SetAddress(request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] KitchenUpdateRequest request, CancellationToken cancellationToken)
        {
            var result = await _kitchenService.UpdateAsync(request ,cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(CancellationToken cancellationToken)
        {
            var result = await _kitchenService.ToggleStatus(cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
