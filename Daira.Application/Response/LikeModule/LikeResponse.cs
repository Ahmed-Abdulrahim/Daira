using Daira.Application.DTOs.LikeModule;

namespace Daira.Application.Response.LikeModule
{
    public class LikeResponse
    {
        public bool Succeeded { get; set; }
        public LikeDto? Like { get; set; }
        public List<LikeDto>? Likes { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static LikeResponse Success(LikeDto like, string message = "Operation completed successfully.")
        {
            return new LikeResponse
            {
                Succeeded = true,
                Like = like,
                Message = message
            };
        }
        public static LikeResponse Success(List<LikeDto> likes, string message = "Operation completed successfully.")
        {
            return new LikeResponse
            {
                Succeeded = true,
                Likes = likes,
                Message = message
            };
        }

        public static LikeResponse Success(string message = "Operation completed successfully.")
        {
            return new LikeResponse
            {
                Succeeded = true,
                Message = message
            };
        }

        public static LikeResponse Failure(string message)
        {
            return new LikeResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static LikeResponse Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new LikeResponse
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
