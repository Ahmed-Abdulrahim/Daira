namespace Daira.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> FindByEmailAsync(string email);
        public Task<bool> FindByUserNameAsync(string userName);
        public Task<AppUser?> CreateUser(AppUser user, string password);
    }
}
