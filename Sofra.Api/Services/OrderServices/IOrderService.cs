using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.CashIn;
using Sofra.Api.Contracts.Order;
using Sofra.Api.Models;

namespace Sofra.Api.Services.OrderServices
{
    public interface IOrderService
    {
        Task<Result<List<OrderResponse>>> GetAllAsync(CancellationToken cancellationToken = default!);

        Task<Result<OrderResponse>> GetAsync(int Id,CancellationToken cancellationToken = default!);

        Task<Result<OrderResponse>> AddAsync(OrderRequest request,CancellationToken cancellationToken = default!);  

        Task<Result<Notification>> ConfirmAsync(int Id);

        Task<Result> DeleteAsync(int Id,CancellationToken cancellationToken = default!);
    }
}
