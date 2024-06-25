namespace Sofra.Api.Models
{
    public class KitchenCategory
    {
        public int Id { get; set; }

        public int KitchenId { get; set; }
        public Kitchen Kitchen { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
