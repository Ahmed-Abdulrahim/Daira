namespace Daira.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailConfirmationAsync(string email, string callbackUrl);
        Task SendPasswordResetEmailAsync(string email, string callbackUrl);
    }
}
