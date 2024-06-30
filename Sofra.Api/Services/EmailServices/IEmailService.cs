using Sofra.Api.Abstractions;

namespace Sofra.Api.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

}
