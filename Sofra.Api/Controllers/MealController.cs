using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Meal;
using Sofra.Api.Services.MealServices;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Kitchen")]
    public class MealController(IMealService mealService) : ControllerBase
    {
        private readonly IMealService _mealService = mealService;


        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int? KitchenId, string? SearchText, CancellationToken cancellationToken)
        {
            var result = await _mealService.GetAllAsync(KitchenId, SearchText, cancellationToken);

            return Ok(result);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mealService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromForm] MealRequest request, CancellationToken cancellationToken)
        {
            var result = await _mealService.AddAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] MealRequest request,CancellationToken cancellationToken)
        {
            var result = await _mealService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mealService.ToggleStatus(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/Delete")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mealService.DeletedAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
