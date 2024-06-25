namespace Sofra.Api.Models
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<KitchenCategory> KitchenCategories { get; set; } = new List<KitchenCategory>();
    }
}
