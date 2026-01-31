namespace Daira.Application.DTOs.AuthDto
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Email Format is InValid")]
        public string Email { get; set; } = string.Empty;
    }
}
