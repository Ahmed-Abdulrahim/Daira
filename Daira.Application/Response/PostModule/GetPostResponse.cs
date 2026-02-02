namespace Daira.Application.Response.PostModule
{
    public class GetPostResponse
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public AuthorResponse Author { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = string.Empty;

        public static GetPostResponse Success(GetPostResponse postResponse)
        {
            return new GetPostResponse
            {
                Id = postResponse.Id,
                Content = postResponse.Content,
                ImageUrl = postResponse.ImageUrl,
                Author = postResponse.Author,
                LikesCount = postResponse.LikesCount,
                CommentsCount = postResponse.CommentsCount,
                CreatedAt = postResponse.CreatedAt,
                Message = "Post Retrived successfully."
            };
        }
        public static GetPostResponse Failure(string message)
        {
            return new GetPostResponse
            {

                Message = message,

            };
        }
    }
}
