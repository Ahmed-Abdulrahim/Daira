namespace Daira.Application.Response
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool RequiresEmailConfirmation { get; set; }
        public List<string> Errors { get; set; }


        public static RegisterResponse Success(string userId, string email, bool requiresConfirmation = true)
        {
            return new RegisterResponse
            {
                Succeeded = true,
                UserId = userId,
                Email = email,
                Message = requiresConfirmation
                    ? "Registration successful. Please check your email to confirm your account."
                    : "Registration successful.",
                RequiresEmailConfirmation = requiresConfirmation
            };
        }

        public static RegisterResponse Failure(IEnumerable<string> errors)
        {
            return new RegisterResponse
            {
                Succeeded = false,
                Message = "Registration failed.",
                Errors = errors.ToList()
            };
        }
    }
}
