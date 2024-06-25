namespace Sofra.Api.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int MealId { get; set; }
        public Meal Meal { get; set; } = default!;

        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
    }
}
