using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Authentication;
using Sofra.Api.Contracts.Customer;
using Sofra.Api.Contracts.Kitchen;

namespace Sofra.Api.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> CustomerRegisterAsync(CustomerRegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> KitchenRegisterAsync(KitchenRegisterRequest request, CancellationToken cancellationToken = default);


}