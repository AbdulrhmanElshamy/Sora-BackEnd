using System.ComponentModel.DataAnnotations;

namespace Sofra.Api.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Comment { get; set; } = null!;

        [Range(0, 5, ErrorMessage = "Rate between 0 | 5")]
        public decimal Rate { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }

        public int CustomerId { get; set; }
        public int KitchenId { get; set; }

        public Customer Customer { get; set; } = default!;
        public Kitchen Kitchen { get; set; } = default!;
    }
}
