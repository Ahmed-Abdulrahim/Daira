namespace Daira.Application.DTOs.AuthDto
{
    public class UpdateProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Bio { get; set; }
        public string DisplayName { get; set; }
        [Url(ErrorMessage = "A valid URL is required.")]
        public string? ProfilePicture { get; set; }
        [Phone(ErrorMessage = "A valid phone number is required.")]
        public string? PhoneNumber { get; set; }
    }
}
