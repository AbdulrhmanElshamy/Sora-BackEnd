namespace Sofra.Api.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = default!;

        public Address? Address { get; set; }

        public Cart? Cart { get; set; }

        public Favorite? Favorite { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
