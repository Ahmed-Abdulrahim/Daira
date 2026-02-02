namespace Daira.Application.Response.PostModule
{
    public class UpdateResponse
    {
        public bool Succeeded { get; set; }
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public AuthorResponse Author { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static UpdateResponse Success(UpdateResponse updateResponse)
        {
            return new UpdateResponse
            {
                Succeeded = true,
                Id = updateResponse.Id,
                Content = updateResponse.Content,
                Author = updateResponse.Author,
                Message = "Post Updated successfully."
            };
        }
        public static UpdateResponse Failure(string message)
        {
            return new UpdateResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static UpdateResponse Failure(IEnumerable<string> errors)
        {
            return new UpdateResponse
            {
                Succeeded = false,
                Errors = errors.ToList(),
            };
        }

    }
}
