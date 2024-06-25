namespace Sofra.Api.Models
{
    public class Meal : AuditableEntity
    {
        public int Id { get; set; }
        
        public string Name { get;set; } = null!;

        public string Description { get; set; } = null!;

        public int PreparationTimeInMinute { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public int KitchenId { get; set; }

        public Kitchen Kitchen { get; set; } = default!;
        public ICollection<MealPhoto> MealPhotos { get; set; } = new List<MealPhoto>();
    }
}
