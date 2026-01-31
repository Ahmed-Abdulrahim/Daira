namespace Domain.Exceptions
{
    public class AuthenticationException : DomainException
    {
        public AuthenticationException(string message, string errorCode = "AuthError")
       : base(message, errorCode)
        {
        }
        public static AuthenticationException InvalidCredentials()
            => new("Invalid email or password.", "AUTH_INVALID_CREDENTIALS");

        public static AuthenticationException AccountLockedOut()
            => new("Account is locked out. Please try again later.", "AUTH_ACCOUNT_LOCKED");
        public static AuthenticationException EmailNotConfirmed()
            => new("Email address has not been confirmed.", "AUTH_EMAIL_NOT_CONFIRMED");
        public static AuthenticationException AccountDeactivated()
            => new("Account has been deactivated.", "AUTH_ACCOUNT_DEACTIVATED");

        public static AuthenticationException InvalidToken()
            => new("The provided token is invalid or has expired.", "AUTH_INVALID_TOKEN");
        public static AuthenticationException InvalidRefreshToken()
            => new("The refresh token is invalid or has expired.", "AUTH_INVALID_REFRESH_TOKEN");

        public static AuthenticationException InvalidTwoFactorCode()
            => new("The two-factor authentication code is invalid.", "AUTH_INVALID_2FA_CODE");
    }
}
