using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;
using X.Paymob.CashIn;
using Sofra.Api.Contracts.CashIn;

namespace Sofra.Api.Services.CashInServices
{
    public class CashInService(IPaymobCashInBroker broker) : ICashInService
    {
        private readonly IPaymobCashInBroker _broker = broker;

        public async Task<string> RequestCardPaymentKey(int amount, CashInRequest request)
        {
            var amountCents = amount;
            var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents);
            var orderResponse = await _broker.CreateOrderAsync(orderRequest);

            var billingData = new CashInBillingData(
                firstName: request.FirstName,
                lastName: request.LastName,
                phoneNumber: request.Phone,
                email: request.Email);

            var paymentKeyRequest = new CashInPaymentKeyRequest(
                integrationId: 4039855,
                orderId: orderResponse.Id,
                billingData: billingData,
                amountCents: amountCents);

            var paymentKeyResponse = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);

            return _broker.CreateIframeSrc(iframeId: "775358", token: paymentKeyResponse.PaymentKey);
        }
    }
}
