namespace Sofra.Api.Contracts.Review
{
    public record ReviewRequest(string Comment,decimal Rate,int kitchenId);
}
