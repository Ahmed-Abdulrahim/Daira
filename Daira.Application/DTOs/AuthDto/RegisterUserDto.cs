namespace Daira.Application.DTOs.AuthDto
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "User Name must between 8 and 50 Character")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores and hyphens")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email Cannot Exceed 100 Character ")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name Is Required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "FirstName Must be Between 3 and 100 Character")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Is Required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "FirstName Must be Between 3 and 100 Character")]
        public string LastName { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "FirstName Must be Between 3 and 100 Character")]
        public string? Bio { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "FirstName Must be Between 3 and 100 Character")]
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }
    }
}
