namespace Daira.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponse> LoginAsync(LoginDto loginDto);
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<LogoutResponse> LogoutAsync(string userId);
        Task<ConfirmEmailResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task<ResendConfirmationResponse> ResendEmailAsync(ResendEmailConfirmationDto resendEmailConfirmationDto);
        Task<ForgetPasswordResponse> ForgetPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ChangePasswordResponse> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<UserProfileResponse> GetProfileAsync(string userId);
        Task<UpdateProfileResponse> UpdateProfileAsync(string userId, UpdateProfileDto dto);


    }
}
