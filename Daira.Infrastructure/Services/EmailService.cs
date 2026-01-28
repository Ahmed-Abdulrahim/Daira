using System.Net;
using System.Net.Mail;

namespace Daira.Infrastructure.Services
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpHost = configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "587");
            var smtpUsername = configuration["Email:SmtpUsername"];
            var smtpPassword = configuration["Email:SmtpPassword"];
            var fromEmail = configuration["Email:FromEmail"];
            var fromName = configuration["Email:FromName"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }

        public async Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            var subject = "Confirm your email";
            var body = $@"
            <html>
            <body>
                <h2>Email Confirmation</h2>
                <p>Please confirm your email by clicking the link below:</p>
                <a href='{callbackUrl}'>Confirm Email</a>
                <p>If you did not create an account, please ignore this email.</p>
            </body>
            </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string callbackUrl)
        {
            var subject = "Reset your password";
            var body = $@"
            <html>
            <body>
                <h2>Password Reset</h2>
                <p>You requested to reset your password. Click the link below to reset it:</p>
                <a href='{callbackUrl}'>Reset Password</a>
                <p>If you did not request a password reset, please ignore this email.</p>
                <p>This link will expire in 24 hours.</p>
            </body>
            </html>";

            await SendEmailAsync(email, subject, body);
        }
    }
}
