namespace Sofra.Api.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsRead { get; set; }

        public int KitchenId { get; set; }
        public Kitchen Kitchen { get; set; } = default!;
    }
}
