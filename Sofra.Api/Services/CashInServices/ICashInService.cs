using Sofra.Api.Contracts.CashIn;

namespace Sofra.Api.Services.CashInServices
{
    public interface ICashInService
    {
        Task<string> RequestCardPaymentKey(int amount, CashInRequest request)
    }
}
