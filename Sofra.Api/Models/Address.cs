namespace Sofra.Api.Models
{
    public class Address
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? KitchenId { get; set; }
        public Kitchen? Kitchen { get; set; }
    }
}
