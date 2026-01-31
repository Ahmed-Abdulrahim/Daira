using Microsoft.AspNetCore.Identity;

namespace Daira.Infrastructure.Services.AuthService
{
    public class AuthService(UserManager<AppUser> userManager, ILogger<AuthService> logger, IMapper mapper, IEmailService emailService, IOptions<EmailSettings> _emailSettings) : IAuthService
    {
        private readonly EmailSettings emailSettings = _emailSettings.Value;
        public async Task<RegisterResponse> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser is not null)
            {
                return RegisterResponse.Failure(new[] { "Email is Already Exist" });
            }
            var user = mapper.Map<AppUser>(registerDto);
            var createUser = await userManager.CreateAsync(user, registerDto.Password);
            if (!createUser.Succeeded)
            {
                var errors = createUser.Errors.Select(e => e.Description);
                return RegisterResponse.Failure(errors);
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{emailSettings.BaseUrl}/api/auth/confirm-email?email={UrlEncoder.Default.Encode(user.Email!)}&token={encodedToken}";

            try
            {
                await emailService.SendEmailConfirmationAsync(user.Email!, confirmationLink);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send confirmation email to {Email}", user.Email);
            }

            logger.LogInformation("User {Email} registered successfully", user.Email);
            return RegisterResponse.Success(user.Id, user.Email!, requiresConfirmation: true);

        }
        public Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ConfirmEmailResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var user = await userManager.FindByEmailAsync(confirmEmailDto.Email);
            if (user == null)
            {
                return ConfirmEmailResponse.Failure("User not found.");
            }

            if (user.EmailConfirmed)
            {
                return ConfirmEmailResponse.AlreadyConfirmed();
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailDto.Token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return ConfirmEmailResponse.Failure(string.Join(", ", errors));
            }

            logger.LogInformation("Email confirmed for user {Email}", user.Email);
            return ConfirmEmailResponse.Success();
        }

        public Task<TwoFactorResponse> DisableTwoFactorAsync(string userId, string code)
        {
            throw new NotImplementedException();
        }

        public Task<TwoFactorResponse> EnableTwoFactorAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ForgetPasswordResponse> ForgetPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileResponse> GetProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<LogoutResponse> LogoutAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }



        public Task<ResendConfirmationResponse> ResendEmailAsync(ResendEmailConfirmationDto resendEmailConfirmationDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateProfileResponse> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> VerifyTwoFactorAsync(TwoFactorDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
