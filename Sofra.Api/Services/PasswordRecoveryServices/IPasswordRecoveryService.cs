using Sofra.Api.Abstractions;

namespace Sofra.Api.Services.PasswordRecoveryServices
{
    public interface IPasswordRecoveryService
    {
        Task<Result> SendRecoveryEmailAsync(string email);
        Task<Result> ResetPasswordAsync(string email,string token, string newPassword);
    }

}
