namespace Sofra.Api.Contracts.Order
{
    public record OrderRequest(IEnumerable<OrderDetail> Details);
}
