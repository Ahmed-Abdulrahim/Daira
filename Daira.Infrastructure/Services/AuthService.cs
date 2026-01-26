namespace Daira.Infrastructure.Services
{
    public class TokenGenerator(UserManager<AppUser> userManager, IConfiguration configration) : ITokenGenerator
    {
        public async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var envSecret = Environment.GetEnvironmentVariable("secretKey");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(envSecret ?? configration["JwtOptions:secretKey"]!));
            var token = new JwtSecurityToken(
                issuer: configration["JwtOptions:issuer"],
                audience: configration["JwtOptions:audience"],
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: claims,
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
