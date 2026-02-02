namespace Daira.Application.Response.FollowerModule
{
    public class FollowerResponse
    {
        public bool Succeeded { get; set; }
        public FollowerDto? Follower { get; set; }
        public List<FollowerDto>? Followers { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static FollowerResponse Success(FollowerDto follwer, string message = "Operation completed successfully.")
        {
            return new FollowerResponse
            {
                Succeeded = true,
                Follower = follwer,
                Message = message
            };
        }
        public static FollowerResponse Success(List<FollowerDto> follwers, string message = "Operation completed successfully.")
        {
            return new FollowerResponse
            {
                Succeeded = true,
                Followers = follwers,
                Message = message
            };
        }

        public static FollowerResponse Success(string message = "Operation completed successfully.")
        {
            return new FollowerResponse
            {
                Succeeded = true,
                Message = message
            };
        }

        public static FollowerResponse Failure(string message)
        {
            return new FollowerResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static FollowerResponse Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new FollowerResponse
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
