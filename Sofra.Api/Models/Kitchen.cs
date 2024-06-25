namespace Sofra.Api.Models
{

    public class Kitchen
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool Enabled { get; set; }

        public double MaxDeliveryDistance { get; set; }

        public string Avatar { get; set; } = null!;

        public Address Address { get; set; } = null!;

        public ApplicationUser ApplicationUser { get; set; } = default!;
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
        public ICollection<KitchenCategory> KitchenCategories { get; set; } = new List<KitchenCategory>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
