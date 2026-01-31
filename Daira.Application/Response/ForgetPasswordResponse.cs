namespace Daira.Application.Response
{
    public class ForgetPasswordResponse
    {
        public bool Succeeded { get; set; }
        public string? Email { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }


        public static ForgetPasswordResponse Success()
        {
            return new ForgetPasswordResponse
            {
                Succeeded = true,
                Message = "Password reset link has been sent to your email.",
            };
        }
        public static ForgetPasswordResponse Success(string email)
        {
            return new ForgetPasswordResponse
            {
                Succeeded = true,
                Email = email,
                Message = "Password reset link has been sent to your email.",
            };
        }

        public static ForgetPasswordResponse Failure(List<string> errors)
        {
            return new ForgetPasswordResponse
            {
                Succeeded = false,
                Message = "Failed to process password reset request.",
                Errors = errors
            };
        }
    }
}
