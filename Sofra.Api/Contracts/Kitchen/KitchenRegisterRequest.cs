namespace Sofra.Api.Contracts.Kitchen
{
    public record KitchenRegisterRequest(string Email,string Password,string FirstName,string LastName,double MaxDeliveryDistance, int[] Categories, string Name,IFormFile Avatar);
}
