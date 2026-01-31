namespace Daira.Application.Response
{
    public class LoginResponse
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool RequiresTwoFactor { get; set; }
        public bool IsLockedOut { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();


        public static LoginResponse Success(string userId, string email, string fullName,
            string accessToken, string refreshToken, DateTime accessTokenExpiration,
            DateTime refreshTokenExpiration, IList<string>? roles = null)
        {
            return new LoginResponse
            {
                Succeeded = true,
                UserId = userId,
                Email = email,
                FullName = fullName,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration,
                Roles = roles?.ToList() ?? new List<string>(),
                Message = "Login successful."
            };
        }

        public static LoginResponse TwoFactorRequired(string email)
        {
            return new LoginResponse
            {
                Succeeded = false,
                Email = email,
                RequiresTwoFactor = true,
                Message = "Two-factor authentication is required."
            };
        }

        public static LoginResponse LockedOut()
        {
            return new LoginResponse
            {
                Succeeded = false,
                IsLockedOut = true,
                Message = "Account is locked out. Please try again later."
            };
        }

        public static LoginResponse Failure(string message)
        {
            return new LoginResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
