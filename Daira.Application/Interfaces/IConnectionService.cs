namespace Daira.Application.Interfaces
{
    public interface IConnectionService
    {
        Task AddConnectionAsync(string userId, string connectionId);
        Task RemoveConnectionAsync(string userId, string connectionId);
        Task<List<string>> GetConnectionsAsync(string userId);
        Task<Dictionary<string, List<string>>> GetConnectionsAsync(IEnumerable<string> userIds);
        Task<bool> IsUserOnlineAsync(string userId);

        Task<List<string>> GetOnlineUsersAsync();
    }
}
