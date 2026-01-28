namespace Daira.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterUserDto registerDto);
        Task<UserResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<bool> RevokeTokenAsync(string userId);
        Task<UserResponseDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        public Task<UserResponseDto> ConfirmEmailAsync(string userId, string token);

    }
}
