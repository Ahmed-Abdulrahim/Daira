namespace Daira.Application.Response
{
    public class RefreshTokenResponse
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }

        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public static RefreshTokenResponse Success(string accessToken, string refreshToken, DateTime accessTokenExpiration, DateTime refreshTokenExpiration)
        {
            return new RefreshTokenResponse
            {
                Succeeded = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration,
                Message = "Token refreshed successfully."
            };
        }

        public static RefreshTokenResponse Failure(string message)
        {
            return new RefreshTokenResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }


    }
}
