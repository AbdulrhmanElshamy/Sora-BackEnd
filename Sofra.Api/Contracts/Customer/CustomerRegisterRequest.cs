namespace Sofra.Api.Contracts.Customer
{
    public record CustomerRegisterRequest(string Email, string Password, string FristName, string LastName);
}
