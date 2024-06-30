using Sofra.Api.Enums;

namespace Sofra.Api.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentType Payment { get; set; } = PaymentType.Cash;

        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public int KitchenId { get; set; }
        public Kitchen Kitchen { get; set; } = default!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
