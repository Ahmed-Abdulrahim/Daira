namespace Daira.Application.Response.Auth
{
    public class UserProfileResponse
    {
        public bool Succeeded { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();


        public static UserProfileResponse Failure(string message)
        {
            return new UserProfileResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
