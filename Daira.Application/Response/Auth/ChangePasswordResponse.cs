namespace Daira.Application.Response.Auth
{
    public class ChangePasswordResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static ChangePasswordResponse Success()
        {
            return new ChangePasswordResponse
            {
                Succeeded = true,
                Message = "Password changed successfully."
            };
        }
        public static ChangePasswordResponse Failure(IEnumerable<string> errors)
        {
            return new ChangePasswordResponse
            {
                Succeeded = false,
                Message = "Failed to change password.",
                Errors = errors.ToList()
            };
        }
        public static ChangePasswordResponse Failure(string error)
        {
            return new ChangePasswordResponse
            {
                Succeeded = false,
                Message = "Failed to change password.",
                Errors = new List<string> { error }
            };
        }


    }
}
