namespace Daira.Infrastructure.Services.AuthService
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings jwtSettings;
        private readonly ILogger<TokenService> logger;
        public TokenService(IOptions<JwtSettings> options, ILogger<TokenService> _logger)
        {
            jwtSettings = options.Value;
            logger = _logger;
        }
        public Task<string> GenerateAccessTokenAsync(AppUser user, IList<string> roles)
        {
            var Claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub , user.Id ),
              new Claim(JwtRegisteredClaimNames.Email , user.Email ?? string.Empty),
              new Claim(JwtRegisteredClaimNames.Jti ,  Guid.NewGuid().ToString()),
              new Claim(ClaimTypes.NameIdentifier  ,user.Id),
              new Claim(ClaimTypes.Email  ,user.Email!),
              new Claim("FirstName" , user.FirstName!),
              new Claim("LastName" , user.LastName!)
            };

            foreach (var role in roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey") ?? jwtSettings.SecretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
           issuer: jwtSettings.Issuer,
           audience: jwtSettings.Audience,
           claims: Claims,
           expires: GetAccessTokenExpiration(),
           signingCredentials: credential
       );
            var writeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(writeToken);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public DateTime GetAccessTokenExpiration()
        {
            return DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // Allow expired tokens
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey") ?? jwtSettings.SecretKey))
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public DateTime GetRefreshTokenExpiration()
        {
            return DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);
        }
    }
}
