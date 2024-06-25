using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Authentication;
using Sofra.Api.Contracts.Customer;
using Sofra.Api.Contracts.Kitchen;
using Sofra.Api.Services;

namespace Sofra.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{

    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] Contracts.Authentication.LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess 
            ? Ok(authResult.Value) 
            : authResult.ToProblem();
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("customer-registration")]
    public async Task<IActionResult> CustomerRegistrationAsync([FromBody] CustomerRegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.CustomerRegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("kitchen-registration")]
    public async Task<IActionResult> KitchenRegistrationAsync([FromForm] KitchenRegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.KitchenRegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}