namespace Daira.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user, IList<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        DateTime GetTokenExpirationTime();

    }
}
