namespace Daira.Application.Response.Roles
{
    public class AssignRoleResponse
    {
        public bool Succeeded { get; set; }
        public string? UserId { get; set; }
        public string? RoleName { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static AssignRoleResponse AssignSuccess(string userId, string roleName)
        {
            return new AssignRoleResponse
            {
                Succeeded = true,
                UserId = userId,
                RoleName = roleName,
                Message = $"Role '{roleName}' assigned successfully."
            };
        }
        public static AssignRoleResponse RemoveSuccess(string userId, string roleName)
        {
            return new AssignRoleResponse
            {
                Succeeded = true,
                UserId = userId,
                RoleName = roleName,
                Message = $"Role '{roleName}' removed successfully."
            };
        }

        public static AssignRoleResponse Failure(string message)
        {
            return new AssignRoleResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
