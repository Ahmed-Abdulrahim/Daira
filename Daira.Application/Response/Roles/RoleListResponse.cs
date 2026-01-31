namespace Daira.Application.Response.Roles
{
    public class RoleListResponse
    {
        public bool Succeeded { get; set; }
        public List<RoleDto> Roles { get; set; } = new();
        public int TotalCount { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static RoleListResponse Success(List<RoleDto> roles)
        {
            return new RoleListResponse
            {
                Succeeded = true,
                Roles = roles,
                TotalCount = roles.Count,
                Message = $"Retrieved {roles.Count} role(s)."
            };
        }
        public static RoleListResponse Failure(string message)
        {
            return new RoleListResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
