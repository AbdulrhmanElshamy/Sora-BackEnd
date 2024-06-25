namespace Sofra.Api.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; } = default!;

        public int MealId { get; set; }
        public Meal Meal { get; set; } = default!;

        public int Quantity { get; set; }
    }
}
