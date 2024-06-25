namespace Sofra.Api.Contracts.Kitchen
{
    public record KitchenUpdateRequest(string Name,double MaxDeliveryDistance, int[] Categories,IFormFile? Avatar);
}
