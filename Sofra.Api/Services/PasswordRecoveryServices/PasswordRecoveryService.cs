using Microsoft.AspNetCore.Identity;
using Sofra.Api.Abstractions;
using Sofra.Api.Errors;
using Sofra.Api.Models;
using Sofra.Api.Services.EmailServices;

namespace Sofra.Api.Services.PasswordRecoveryServices
{
    public class PasswordRecoveryService(IEmailService emailService, UserManager<ApplicationUser> userManager) : IPasswordRecoveryService
    {
        private readonly IEmailService _emailService = emailService;
        private readonly UserManager<ApplicationUser> _userManager = userManager; 

        public async Task<Result> SendRecoveryEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return Result.Failure(UserErrors.InvalidCredentials);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetLink = $"http://localhost:3000/reset-password?Email={email}&Token={token}";
            await _emailService.SendEmailAsync(email, "Reset Your Password", $"Reset your password by <a href='{passwordResetLink}'>clicking here</a>.");
            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(string email, string token,string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return Result.Failure(UserErrors.InvalidCredentials);

             var result = await userManager.ResetPasswordAsync(user,token ,newPassword);
            if(result.Succeeded) return Result.Success();

            return Result.Failure(UserErrors.InvalidJwtToken);
        }
    }
}
