namespace Daira.Application.Response.PostModule
{
    public class PostResponse
    {
        public bool Succeeded { get; set; }
        public GetPostResponse? Post { get; set; }
        public List<GetPostResponse>? Posts { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static PostResponse Success(GetPostResponse post, string message = "Operation completed successfully.")
        {
            return new PostResponse
            {
                Succeeded = true,
                Post = post,
                Message = message
            };
        }
        public static PostResponse Success(List<GetPostResponse> posts, string message = "Operation completed successfully.")
        {
            return new PostResponse
            {
                Succeeded = true,
                Posts = posts,
                Message = message
            };
        }

        public static PostResponse Success(string message = "Operation completed successfully.")
        {
            return new PostResponse
            {
                Succeeded = true,
                Message = message
            };
        }

        public static PostResponse Failure(string message)
        {
            return new PostResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static PostResponse Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new PostResponse
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
