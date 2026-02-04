namespace Daira.Infrastructure.Services
{
    public class ConnectionService : IConnectionService
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> connection = new();
        private static readonly object _lock = new();

        //Add Connection
        public Task AddConnectionAsync(string userId, string connectionId)
        {
            lock (_lock)
            {
                if (!connection.TryGetValue(userId, out var connections))
                {
                    connections = new HashSet<string>();
                    connection[userId] = connections;
                }
                connections.Add(connectionId);
            }
            return Task.CompletedTask;
        }

        // Get All Connections Of Specific User
        public Task<List<string>> GetConnectionsAsync(string userId)
        {
            if (connection.TryGetValue(userId, out var connections))
            {
                return Task.FromResult(connections.ToList());
            }
            return Task.FromResult(new List<string>());
        }

        // Get All Connections Of All User
        public Task<Dictionary<string, List<string>>> GetConnectionsAsync(IEnumerable<string> userIds)
        {
            var result = new Dictionary<string, List<string>>();
            foreach (var userId in userIds)
            {
                if (connection.TryGetValue(userId, out var connections))
                {
                    result[userId] = connections.ToList();
                }
            }
            return Task.FromResult(result);
        }

        //Get All Online User
        public Task<List<string>> GetOnlineUsersAsync()
        {
            return Task.FromResult(connection.Keys.ToList());
        }

        //Is User Online
        public Task<bool> IsUserOnlineAsync(string userId)
        {
            return Task.FromResult(connection.ContainsKey(userId) && connection[userId].Count > 0);
        }

        // Remove Connection of Specific User
        public Task RemoveConnectionAsync(string userId, string connectionId)
        {
            lock (_lock)
            {
                if (connection.TryGetValue(userId, out var connections))
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        connection.TryRemove(userId, out _);
                    }
                }
                return Task.CompletedTask;
            }
        }
    }
}
