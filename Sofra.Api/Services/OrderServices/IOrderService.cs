using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Order;

namespace Sofra.Api.Services.OrderServices
{
    public interface IOrderService
    {
        Task<Result<OrderResponse>> AddAsync(OrderRequest request,CancellationToken cancellationToken = default!);  

        Task<Result> DeleteAsync(int Id,CancellationToken cancellationToken = default!);
    }
}
