namespace Daira.Application.Response.CommentModule
{
    public class CommentResponse
    {
        public bool Succeeded { get; set; }
        public CommentDto? Comment { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static CommentResponse Success(CommentDto comment, string message = "Operation completed successfully.")
        {
            return new CommentResponse
            {
                Succeeded = true,
                Comment = comment,
                Message = message
            };
        }
        public static CommentResponse Success(List<CommentDto> comments, string message = "Operation completed successfully.")
        {
            return new CommentResponse
            {
                Succeeded = true,
                Comments = comments,
                Message = message
            };
        }

        public static CommentResponse Failure(string message)
        {
            return new CommentResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static CommentResponse Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new CommentResponse
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
