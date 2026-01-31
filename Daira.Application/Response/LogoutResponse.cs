namespace Daira.Application.Response
{
    public class LogoutResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }


        public static LogoutResponse Succedd()
        {
            return new LogoutResponse
            {
                Succeeded = true,
                Message = "Logout successful."
            };
        }

        public static LogoutResponse Failed(string errorMessage)
        {
            return new LogoutResponse
            {
                Succeeded = false,
                Message = errorMessage
            };
        }
    }
}
