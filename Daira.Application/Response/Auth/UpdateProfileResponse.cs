namespace Daira.Application.Response.Auth
{
    public class UpdateProfileResponse
    {
        public bool Succeeded { get; set; }
        public UserProfileResponse Profile { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = new();
        public static UpdateProfileResponse Success(UserProfileResponse profile)
        {
            return new UpdateProfileResponse
            {
                Succeeded = true,
                Profile = profile,
                Message = "Profile has been updated successfully."
            };
        }

        public static UpdateProfileResponse Success()
        {
            return new UpdateProfileResponse
            {
                Succeeded = true,
                Message = "Profile has been updated successfully."
            };
        }
        public static UpdateProfileResponse Failure(string message)
        {
            return new UpdateProfileResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static UpdateProfileResponse Failure(IEnumerable<string> errors)
        {
            return new UpdateProfileResponse
            {
                Succeeded = false,
                Message = "Profile update failed.",
                Errors = errors.ToList()
            };
        }
    }
}
