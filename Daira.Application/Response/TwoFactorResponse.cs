namespace Daira.Application.Response
{
    public class TwoFactorResponse
    {
        public bool Succeeded { get; set; }
        public bool IsTwoFactorEnabled { get; set; }

        public string? AuthenticatorKey { get; set; }
        public string? QrCodeUrl { get; set; }
        public List<string> RecoveryCodes { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static TwoFactorResponse EnableSuccess(string authenticatorKey, string qrCodeUrl, IEnumerable<string> recoveryCodes)
        {
            return new TwoFactorResponse
            {
                Succeeded = true,
                IsTwoFactorEnabled = true,
                AuthenticatorKey = authenticatorKey,
                QrCodeUrl = qrCodeUrl,
                RecoveryCodes = recoveryCodes.ToList(),
                Message = "Two-factor authentication has been enabled successfully."
            };
        }
        public static TwoFactorResponse DisableSuccess()
        {
            return new TwoFactorResponse
            {
                Succeeded = true,
                IsTwoFactorEnabled = false,
                Message = "Two-factor authentication has been disabled."
            };
        }

        public static TwoFactorResponse VerificationSuccess()
        {
            return new TwoFactorResponse
            {
                Succeeded = true,
                Message = "Two-factor authentication verified successfully."
            };
        }
        public static TwoFactorResponse Failure(string message)
        {
            return new TwoFactorResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
