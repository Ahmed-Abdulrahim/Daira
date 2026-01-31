namespace Daira.Application.DTOs.AuthDto
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Access token is required.")]
        public string Token { get; set; } = string.Empty;
        [Required(ErrorMessage = "RefreshToken token is required.")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
