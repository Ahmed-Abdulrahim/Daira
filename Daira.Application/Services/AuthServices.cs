namespace Daira.Application.Services
{
    public class AuthService(UserManager<AppUser> userManager, ITokenService tokenService) : IAuthService
    {
        public async Task<UserResponseDto> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedAt = DateTime.Now,
                IsActive = true,
                Bio = registerDto.Bio,
                DisplayName = registerDto.DisplayName

            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = string.Join(",", result.Errors.Select(e => e.Description))
                };
            }

            await userManager.AddToRoleAsync(user, "User");

            var roles = await userManager.GetRolesAsync(user);
            var accessToken = tokenService.GenerateAccessToken(user, roles);
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new UserResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiration = tokenService.GetTokenExpirationTime(),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                }
            };
        }
        public async Task<UserResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            if (principal == null)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "Invalid access token"
                };
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "Invalid token claims"
                };
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null ||
                user.RefreshToken != refreshTokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var roles = await userManager.GetRolesAsync(user);
            var newAccessToken = tokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new UserResponseDto
            {
                Success = true,
                Message = "Token refreshed successfully",
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpiration = tokenService.GetTokenExpirationTime()
            };
        }
        public async Task<bool> RevokeTokenAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return true;
        }
        public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
        {
            var existenceEmail = await userManager.FindByEmailAsync(loginDto.Email);
            if (existenceEmail is null)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "User with this email don't exists"
                };
            }
            var checkPassword = await userManager.CheckPasswordAsync(existenceEmail, loginDto.Password);
            if (!checkPassword)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }
            var roles = await userManager.GetRolesAsync(existenceEmail);
            var accessToken = tokenService.GenerateAccessToken(existenceEmail, roles);
            var refreshToken = tokenService.GenerateRefreshToken();

            return new UserResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiration = tokenService.GetTokenExpirationTime(),
                User = new UserDto
                {
                    Id = existenceEmail.Id,
                    Email = existenceEmail.Email!,
                    FirstName = existenceEmail.FirstName,
                    LastName = existenceEmail.LastName,
                    PhoneNumber = existenceEmail.PhoneNumber,
                    Roles = roles.ToList()
                }
            };

        }
        public async Task<UserResponseDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var existenceUser = await userManager.FindByIdAsync(userId);
            if (existenceUser is null)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var changePassword = await userManager.ChangePasswordAsync(existenceUser,
                changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!changePassword.Succeeded)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = string.Join(",", changePassword.Errors.Select(e => e.Description))
                };

            }
            existenceUser.RefreshToken = null;
            existenceUser.RefreshTokenExpiryTime = null;
            await userManager.UpdateAsync(existenceUser);
            return new UserResponseDto
            {
                Success = true,
                Message = "Password changed successfully ,please login again"
            };

        }
    }

}
