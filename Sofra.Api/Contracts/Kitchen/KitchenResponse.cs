namespace Sofra.Api.Contracts.Kitchen
{
    public record KitchenResponse(int Id , string Name , string Avatar , bool Enabled, string Latitude,string Longitude, double MaxDeliveryDistance);
}
