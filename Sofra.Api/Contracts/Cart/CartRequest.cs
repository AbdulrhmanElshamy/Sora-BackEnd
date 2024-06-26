namespace Sofra.Api.Contracts.Cart
{
    public record CartRequest(IEnumerable<CartItem> Items);
}
