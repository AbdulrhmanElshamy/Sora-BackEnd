namespace Sofra.Api.Models
{
    public class MealPhoto
    {
        public int Id { get; set; }
    
        public string Image {  get; set; } =null!;

        public int MealId { get; set; }
        public Meal Meal { get; set; } = default!;
    }
}
