namespace Daira.Application.Response.Roles
{
    public class UsersInRoleResponse
    {
        public bool Succeeded { get; set; }
        public string? RoleName { get; set; }
        public List<UserRoleDto> Users { get; set; } = new();

        public int TotalCount { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static UsersInRoleResponse Success(string roleName, List<UserRoleDto> users)
        {
            return new UsersInRoleResponse
            {
                Succeeded = true,
                RoleName = roleName,
                Users = users,
                TotalCount = users.Count,
                Message = $"Found {users.Count} user(s) in role '{roleName}'."
            };
        }

        public static UsersInRoleResponse Failure(string message)
        {
            return new UsersInRoleResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
