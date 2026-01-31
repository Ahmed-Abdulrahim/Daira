namespace Daira.Application.Response
{
    public class ResendConfirmationResponse
    {
        public bool Succeeded { get; set; }

        public string? Email { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = new();

        public static ResendConfirmationResponse Success(string email)
        {
            return new ResendConfirmationResponse
            {
                Succeeded = true,
                Email = email,
                Message = "If an account with that email exists and is not confirmed, a new confirmation email has been sent."
            };
        }

        public static ResendConfirmationResponse Success()
        {
            return new ResendConfirmationResponse
            {
                Succeeded = true,
                Message = "If an account with that email exists and is not confirmed, a new confirmation email has been sent."
            };
        }
        public static ResendConfirmationResponse AlreadyConfirmed()
        {
            return new ResendConfirmationResponse
            {
                Succeeded = true,
                Message = "Email address has already been confirmed."
            };
        }
        public static ResendConfirmationResponse Failure(string message)
        {
            return new ResendConfirmationResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
