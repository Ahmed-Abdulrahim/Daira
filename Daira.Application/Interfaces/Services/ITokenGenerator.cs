namespace Daira.Application.Interfaces.Services
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateJwtToken(AppUser user);

    }
}
