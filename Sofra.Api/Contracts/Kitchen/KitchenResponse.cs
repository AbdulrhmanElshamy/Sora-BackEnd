namespace Sofra.Api.Contracts.Kitchen
{
    public record KitchenResponse(int Id , string Name , string Avatar , bool Enabled,decimal Rate, string Latitude,string Longitude, double MaxDeliveryDistance);
}
