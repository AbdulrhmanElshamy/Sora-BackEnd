namespace Sofra.Api.Contracts.Order
{
    public record OrderResponse(int Id,DateTime CreatedOn,string Status,string Payment,decimal TotalPrice,string Notes,IEnumerable<OrderDetail> Details);
}