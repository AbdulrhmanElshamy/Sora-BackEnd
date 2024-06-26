namespace Sofra.Api.Contracts.Cart
{
    public record CartResponse(int Id, IEnumerable<CartItem> Items);
}
