namespace Sofra.Api.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);