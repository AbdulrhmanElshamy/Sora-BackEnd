using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Favorite;
using Sofra.Api.Services.FavoriteServices;

namespace Sofra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class FavoriteController(IFavoriteService _favoriteService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _favoriteService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromBody] FavoriteRequest request, CancellationToken cancellationToken)
        {
            var result = await _favoriteService.AddAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }
    }
}
