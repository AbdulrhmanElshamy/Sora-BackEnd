using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Category;
using Sofra.Api.Services.CategoryServices;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _categoryService.GetAllAsync(cancellationToken));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.AddAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _categoryService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.ToggleStatusAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
