namespace Daira.Application.Response.PostModule
{
    public class CreatePostResponse
    {
        public bool Succeeded { get; set; }
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public AuthorResponse Author { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLikedByMe { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static CreatePostResponse Success(CreatePostResponse createPostResponse)
        {
            return new CreatePostResponse
            {
                Succeeded = true,
                Id = createPostResponse.Id,
                Content = createPostResponse.Content,
                ImageUrl = createPostResponse.ImageUrl,
                Author = createPostResponse.Author,
                LikesCount = createPostResponse.LikesCount,
                CommentsCount = createPostResponse.CommentsCount,
                IsLikedByMe = createPostResponse.IsLikedByMe,
                CreatedAt = createPostResponse.CreatedAt,
                Message = "Post created successfully."
            };
        }
        public static CreatePostResponse Failure(string message)
        {
            return new CreatePostResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static CreatePostResponse Failure(IEnumerable<string> errors)
        {
            return new CreatePostResponse
            {
                Succeeded = false,
                Errors = errors.ToList(),
            };
        }

    }
}