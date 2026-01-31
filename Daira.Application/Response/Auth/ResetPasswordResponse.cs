namespace Daira.Application.Response.Auth
{
    public class ResetPasswordResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static ResetPasswordResponse Success()
        {
            return new ResetPasswordResponse
            {
                Succeeded = true,
                Message = "Password has been reset successfully. You can now log in with your new password."
            };
        }
        public static ResetPasswordResponse Failure(IEnumerable<string> errors)
        {
            return new ResetPasswordResponse
            {
                Succeeded = false,
                Message = "Password reset failed.",
                Errors = errors.ToList()
            };
        }
        public static ResetPasswordResponse Failure(string error)
        {
            return new ResetPasswordResponse
            {
                Succeeded = false,
                Message = error,
                Errors = new List<string> { error }
            };
        }
    }
}
