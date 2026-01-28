namespace Daira.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByIdAsync(string id);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<bool> ExistsAsync(string email);
        Task UpdateAsync(AppUser user);
    }
}
