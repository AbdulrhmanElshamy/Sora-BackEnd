using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Cart;

namespace Sofra.Api.Services.CartServices
{
    public interface ICartService
    {
        Task<Result<CartResponse>> GetAsync(int id,CancellationToken cancellationToken = default!);

        Task<Result<CartResponse>> AddAsync(CartRequest request,CancellationToken cancellationToken = default!);
    }
}
