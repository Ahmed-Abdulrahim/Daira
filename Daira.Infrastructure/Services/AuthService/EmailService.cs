namespace Daira.Infrastructure.Services.AuthService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<EmailSettings> _emailSetting, ILogger<EmailService> _logger)
        {
            emailSettings = _emailSetting.Value;
            logger = _logger;
        }


        public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            var subject = "Confirm Your Email Address";
            var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Email Confirmation</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2563eb;'>Welcome!</h2>
                    <p>Thank you for registering. Please confirm your email address by clicking the button below:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{confirmationLink}' 
                           style='background-color: #2563eb; color: white; padding: 12px 30px; 
                                  text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Confirm Email
                        </a>
                    </div>
                    <p>If the button doesn't work, copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #666;'>{confirmationLink}</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
                    <p style='font-size: 12px; color: #666;'>
                        If you didn't create an account, please ignore this email.
                    </p>
                </div>
            </body>
            </html>";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetAsync(string email, string resetLink)
        {

            var subject = "Reset Your Password";
            var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Password Reset</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2563eb;'>Password Reset Request</h2>
                    <p>We received a request to reset your password. Click the button below to create a new password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #2563eb; color: white; padding: 12px 30px; 
                                  text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Reset Password
                        </a>
                    </div>
                    <p>If the button doesn't work, copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #666;'>{resetLink}</p>
                    <p style='color: #dc2626; font-weight: bold;'>This link will expire in 24 hours.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
                    <p style='font-size: 12px; color: #666;'>
                        If you didn't request a password reset, please ignore this email or contact support.
                    </p>
                </div>
            </body>
            </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendTwoFactorCodeAsync(string email, string code)
        {
            var subject = "Your Two-Factor Authentication Code";
            var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Two-Factor Authentication</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2563eb;'>Two-Factor Authentication</h2>
                    <p>Your verification code is:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='font-size: 32px; font-weight: bold; letter-spacing: 8px; 
                                    background-color: #f3f4f6; padding: 15px 30px; border-radius: 8px;'>
                            {code}
                        </span>
                    </div>
                    <p style='color: #dc2626;'>This code will expire in 10 minutes.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
                    <p style='font-size: 12px; color: #666;'>
                        If you didn't request this code, please secure your account immediately.
                    </p>
                </div>
            </body>
            </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
                message.To.Add(new MailboxAddress(string.Empty, to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                var secureSocketOptions = emailSettings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                await client.ConnectAsync(emailSettings.SmtpHost, emailSettings.SmtpPort, secureSocketOptions);
                if (!String.IsNullOrEmpty(emailSettings.SmtpUser))
                {
                    await client.AuthenticateAsync(emailSettings.SmtpUser, emailSettings.SmtpPassword);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                logger.LogInformation("Email sent successfully to {Email}", to);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {Email}", to);
                throw;
            }
        }
    }
}
