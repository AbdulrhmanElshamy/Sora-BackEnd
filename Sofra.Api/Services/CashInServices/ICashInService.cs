using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.CashIn;

namespace Sofra.Api.Services.CashInServices
{
    public interface ICashInService
    {
        Task<Result<string>> RequestCardPaymentKey(int amount, CashInRequest request,CancellationToken cancellationToken = default!);
        Task<Result<bool>> ValidateHmac(PayMobCallbackRequest request);
    }
}
