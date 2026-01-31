namespace Daira.Application.Response.Roles
{
    public class RoleResponse
    {
        public bool Succeeded { get; set; }
        public RoleDto? Role { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static RoleResponse Success(RoleDto role, string message = "Operation completed successfully.")
        {
            return new RoleResponse
            {
                Succeeded = true,
                Role = role,
                Message = message
            };
        }

        public static RoleResponse Failure(string message)
        {
            return new RoleResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static RoleResponse Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new RoleResponse
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
