namespace Sofra.Api.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public ICollection<FavoriteItem> Items { get; set; } = new List<FavoriteItem>();
    }
}
