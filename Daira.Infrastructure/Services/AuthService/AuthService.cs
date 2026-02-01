namespace Daira.Infrastructure.Services.AuthService
{
    public class AuthService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, ITokenService tokenService, ILogger<AuthService> logger, IMapper mapper, IEmailService emailService, IOptions<EmailSettings> _emailSettings) : IAuthService
    {
        private readonly EmailSettings emailSettings = _emailSettings.Value;

        //Register User
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

        //Login User
        public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return LoginResponse.Failure("Invalid email or password.");

            if (!user.IsActive) return LoginResponse.Failure("User account is deactivated.");

            if (!user.EmailConfirmed) return LoginResponse.Failure("Email is not confirmed.");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                return LoginResponse.LockedOut();
            }

            if (result.RequiresTwoFactor)
            {
                return LoginResponse.TwoFactorRequired(user.Email!);
            }

            if (!result.Succeeded)
            {
                return LoginResponse.Failure("Invalid email or password.");
            }

            return await GenerateAuthTokensAsync(user);
        }

        //Change Password
        public async Task<ChangePasswordResponse> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return ChangePasswordResponse.Failure("User not found.");
            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return ChangePasswordResponse.Failure(errors);
            }
            var specRefreshTokens = new RefreshTokenSpecification(rt => rt.UserId == user.Id);
            await LogoutAsync(userId);
            logger.LogInformation("Password changed for user {UserId} , Plz Login again", userId);
            return ChangePasswordResponse.Success();
        }

        //Confirm Email
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

        //Forget Password
        public async Task<ForgetPasswordResponse> ForgetPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user is null || !user.EmailConfirmed)
            {
                return ForgetPasswordResponse.Success();
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink = $"{emailSettings.BaseUrl}/api/auth/reset-password?email={UrlEncoder.Default.Encode(user.Email!)}&token={encodedToken}";

            try
            {
                await emailService.SendPasswordResetAsync(user.Email!, resetLink);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send password reset email to {Email}", user.Email);
            }
            return ForgetPasswordResponse.Success();
        }

        //Get User Profile
        public async Task<UserProfileResponse> GetProfileAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return UserProfileResponse.Failure("User not found.");
            return mapper.Map<UserProfileResponse>(user); ;
        }

        //LogOut User
        public async Task<LogoutResponse> LogoutAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return LogoutResponse.Failed("User not found.");
            var spec = new RefreshTokenSpecification(rt => rt.UserId == user.Id && rt.IsRevoked == false && rt.ExpiresAt >= DateTime.UtcNow);
            var refreshTokens = await unitOfWork.Repository<RefreshToken>().GetAllWithSpec(spec);
            foreach (var token in refreshTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }
            await unitOfWork.CommitAsync();
            return LogoutResponse.Succedd();

        }

        //Genrate New Tokens
        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            if (principal is null) return RefreshTokenResponse.Failure("Invalid access token.");
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return RefreshTokenResponse.Failure("Invalid access token.");
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return RefreshTokenResponse.Failure("User not found.");
            var specRefreshToken = new RefreshTokenSpecification(rt => rt.Token == refreshTokenDto.RefreshToken && rt.UserId == user.Id && rt.ExpiresAt >= DateTime.UtcNow && rt.IsRevoked == false);
            var getRefreshToken = await unitOfWork.Repository<RefreshToken>().GetByIdSpecTracked(specRefreshToken);
            if (getRefreshToken is null) return RefreshTokenResponse.Failure("Invalid refresh token.");
            var roles = await userManager.GetRolesAsync(user);
            var newAccessToken = await tokenService.GenerateAccessTokenAsync(user, roles);
            var newRefeshToken = tokenService.GenerateRefreshToken();
            var refreshTokenrow = new RefreshToken
            {
                Token = newRefeshToken,
                ExpiresAt = tokenService.GetRefreshTokenExpiration(),
                UserId = user.Id,
            };
            getRefreshToken.IsRevoked = true;
            getRefreshToken.RevokedAt = DateTime.UtcNow;
            unitOfWork.Repository<RefreshToken>().Update(getRefreshToken);
            await unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenrow);
            await unitOfWork.CommitAsync();
            return RefreshTokenResponse.Success(
                newAccessToken,
                newRefeshToken,
                tokenService.GetAccessTokenExpiration(),
                tokenService.GetRefreshTokenExpiration());
        }

        //Resend Email Confirmation
        public async Task<ResendConfirmationResponse> ResendEmailAsync(ResendEmailConfirmationDto resendEmailConfirmationDto)
        {
            var user = await userManager.FindByEmailAsync(resendEmailConfirmationDto.Email);
            if (user is null) return ResendConfirmationResponse.Failure("User not Found");
            if (!user.EmailConfirmed) return ResendConfirmationResponse.AlreadyConfirmed();
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{emailSettings.BaseUrl}/api/auth/confirm-email?email={UrlEncoder.Default.Encode(user.Email!)}&token={encodedToken}";

            try
            {
                await emailService.SendEmailConfirmationAsync(user.Email!, confirmationLink);
            }
            catch
            {
                logger.LogError("Failed to send confirmation email to {Email}", user.Email);
            }
            return ResendConfirmationResponse.Success(user.Email!);
        }

        //ResetPassword
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return ResetPasswordResponse.Failure("Invalid request.");
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.Token));
            var result = await userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return ResetPasswordResponse.Failure(string.Join(", ", errors));
            }

            await LogoutAsync(user.Id);

            logger.LogInformation("Password reset successfully for user {Email}", user.Email);
            return ResetPasswordResponse.Success();
        }

        //Update User Profile
        public async Task<UpdateProfileResponse> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return UpdateProfileResponse.Failure("User not found.");
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.PictureUrl = dto.ProfilePicture;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return UpdateProfileResponse.Failure(string.Join(", ", errors));
            }

            logger.LogInformation("Profile updated for user {UserId}", userId);
            return UpdateProfileResponse.Success();
        }


        //Private Methods
        private async Task<LoginResponse> GenerateAuthTokensAsync(AppUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var accessToken = await tokenService.GenerateAccessTokenAsync(user, roles);
            var refreshToken = tokenService.GenerateRefreshToken();


            var refreshTokenrow = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = tokenService.GetRefreshTokenExpiration(),
                UserId = user.Id,
            };
            await unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenrow);
            await unitOfWork.CommitAsync();

            return LoginResponse.Success(
                user.Id,
                user.Email!,
                user.FullName,
                accessToken,
                tokenService.GetAccessTokenExpiration(),
                tokenService.GetRefreshTokenExpiration(),
                 refreshToken,
                roles);
        }



    }
}
