using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Category;
using Sofra.Api.Contracts.Review;
using Sofra.Api.Services.ReviewServices;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService reviewServices) : ControllerBase
    {
        private readonly IReviewService _reviewServices = reviewServices;

        [HttpGet("GetAll/{KitchenId}")]
        public async Task<IActionResult> GetAll([FromRoute] int KitchenId, CancellationToken cancellationToken)
        {
            return Ok(await _reviewServices.GetAllAsync(KitchenId,cancellationToken));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _reviewServices.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromBody] ReviewRequest request, CancellationToken cancellationToken)
        {
            var result = await _reviewServices.AddAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ReviewRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _reviewServices.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
