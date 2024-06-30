using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;
using X.Paymob.CashIn;
using Sofra.Api.Contracts.CashIn;
using Sofra.Api.Abstractions;
using Sofra.Api.Errors;
using Sofra.Api.Data;
using System.Security.Cryptography;
using System.Text;

namespace Sofra.Api.Services.CashInServices
{
    public class CashInService(IPaymobCashInBroker broker,ApplicationDbContext dbContext) : ICashInService
    {
        private readonly IPaymobCashInBroker _broker = broker;
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result<string>> RequestCardPaymentKey(int orderId, CashInRequest request, CancellationToken cancellationToken = default!)
        {
            var order = await _dbContext.Orders.FindAsync(orderId, cancellationToken);

            if (order is null)
                return Result.Failure<string>(OrderErrors.OrderNotFound);

            int amount =  (int)order.TotalPrice;

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

            return Result.Success(_broker.CreateIframeSrc(iframeId: "775358", token: paymentKeyResponse.PaymentKey));
        }

        public async Task<Result<bool>> ValidateHmac(PayMobCallbackRequest request)
        {
            var hmacSecret = "50DE1FC724B86E2B1DDD6F94DD7CA71F";
            var concatenatedString = $"{request.Id}{request.Order}{request.AmountCents}{request.Currency}{request.PaymentStatus}";
            var computedHmac = ComputeHmacSha512(concatenatedString, hmacSecret);

            return Result.Success(computedHmac == request.Hmac);
        }

        private string ComputeHmacSha512(string data, string key)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
