namespace Sofra.Api.Contracts.CashIn
{
    public record PayMobCallbackRequest(int Id,string Order,string AmountCents,string Currency,string PaymentStatus,string Hmac);
}