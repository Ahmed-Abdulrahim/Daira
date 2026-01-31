namespace Daira.Application.Response.Roles
{
    public class UserRolesResponse
    {
        public bool Succeeded { get; set; }

        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static UserRolesResponse Success(string userId, string email, string fullName, IList<string> roles)
        {
            return new UserRolesResponse
            {
                Succeeded = true,
                UserId = userId,
                Email = email,
                FullName = fullName,
                Roles = roles.ToList(),
                Message = "User roles retrieved successfully."
            };
        }

        public static UserRolesResponse Failure(string message)
        {
            return new UserRolesResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
