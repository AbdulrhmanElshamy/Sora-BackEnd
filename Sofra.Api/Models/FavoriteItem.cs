namespace Sofra.Api.Models
{
    public class FavoriteItem
    {
        public int Id { get; set; }

        public int FavoriteId { get; set; }
        public Favorite Favorite { get; set; } = default!;

        public int MealId { get; set; }
        public Meal Meal { get; set; } = default!;
    }
}
