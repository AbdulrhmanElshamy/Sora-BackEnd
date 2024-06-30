namespace Sofra.Api.Contracts.Review
{
    public record ReviewResponse(int Id,string Comment, decimal Rate,DateTime CreatedOn);
}