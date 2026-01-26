namespace Daira.Infrastructure.Repositories
{
    public class UserRepository(UserManager<AppUser> userManager) : IUserRepository
    {
        public async Task<AppUser?> CreateUser(AppUser user, string password) => await userManager.CreateAsync(user, password) is IdentityResult result && result.Succeeded ? user : null;

        public async Task<bool> FindByEmailAsync(string email) => await userManager.FindByEmailAsync(email) is not null;
        public async Task<bool> FindByUserNameAsync(string userName) => await userManager.FindByNameAsync(userName) is not null;

    }
}
