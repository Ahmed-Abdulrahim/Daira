namespace Daira.Application.Response.FriendshipModule
{
    public class FriendshipResponse
    {
        public bool Succeeded { get; set; }
        public FriendshipDto? friendshipDto { get; set; }
        public List<FriendshipDto>? friendshipDtos { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static FriendshipResponse Success(FriendshipDto friendshipDto, string message = "Operation completed successfully.")
        {
            return new FriendshipResponse
            {
                Succeeded = true,
                friendshipDto = friendshipDto,
                Message = message
            };
        }
        public static FriendshipResponse Success(List<FriendshipDto> friendshipDtos, string message = "Operation completed successfully.")
        {
            return new FriendshipResponse
            {
                Succeeded = true,
                friendshipDtos = friendshipDtos,
                Message = message
            };
        }

        public static FriendshipResponse Success(string message = "Operation completed successfully.")
        {
            return new FriendshipResponse
            {
                Succeeded = true,
                Message = message
            };
        }

        public static FriendshipResponse Failure(string message)
        {
            return new FriendshipResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static FriendshipResponse Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new FriendshipResponse
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
