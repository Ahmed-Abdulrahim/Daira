namespace Daira.Application.DTOs.AuthDto
{
    public class UserResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDto? User { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public List<string>? Errors { get; set; }
    }
}
