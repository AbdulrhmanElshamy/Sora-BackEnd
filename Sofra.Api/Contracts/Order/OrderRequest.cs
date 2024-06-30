using Sofra.Api.Enums;

namespace Sofra.Api.Contracts.Order
{
    public record OrderRequest(PaymentType PaymentType,string Notes, IEnumerable<OrderDetail> Details);
}
