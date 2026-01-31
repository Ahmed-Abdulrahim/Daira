namespace Daira.Application.Response
{
    public class ConfirmEmailResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public List<string> Error { get; set; }


        public static ConfirmEmailResponse Success()
        {
            return new ConfirmEmailResponse
            {
                Succeeded = true,
                Message = "Email confirmed successfully, You Can Now Login",
            };
        }

        public static ConfirmEmailResponse Success(string email)
        {
            return new ConfirmEmailResponse
            {
                Succeeded = true,
                Email = email,
                Message = "Email confirmed successfully, You Can Now Login",
            };
        }

        public static ConfirmEmailResponse AlreadyConfirmed(string email)
        {
            return new ConfirmEmailResponse
            {
                Succeeded = true,
                Email = email,
                Message = "Email is already confirmed.",
            };
        }

        public static ConfirmEmailResponse Failure(string email, List<string> errors)
        {
            return new ConfirmEmailResponse
            {
                Succeeded = false,
                Message = "Email confirmation failed.",
                Email = email,
                Error = errors
            };
        }
    }
}
