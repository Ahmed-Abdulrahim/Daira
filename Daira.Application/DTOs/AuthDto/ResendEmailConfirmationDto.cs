namespace Daira.Application.DTOs.AuthDto
{
    public class ResendEmailConfirmationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "InValid Email Format")]
        public string Email { get; set; } = string.Empty;
    }
}
