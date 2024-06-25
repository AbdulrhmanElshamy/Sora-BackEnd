namespace Sofra.Api.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
