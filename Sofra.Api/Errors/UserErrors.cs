using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail =
    new("Email.DuplicatedTitle", "Another Account with the same Email is already exists", StatusCodes.Status409Conflict);

    public static readonly Error DuplicatedPhone =
new("PhoneNumber.DuplicatedTitle", "Another Account with the same PhoneNumber is already exists", StatusCodes.Status409Conflict);
}