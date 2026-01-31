namespace Daira.Application.Validation
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.UserName).NotEmpty().WithMessage("UserName is Required")
                .MinimumLength(20).MaximumLength(50).WithMessage("User Name must between 8 and 50 Character")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Username can only contain letters, numbers, underscores and hyphens");

            RuleFor(r => r.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email Format")
                .MaximumLength(256).WithMessage("Email Cannot Exceed 100 Character ");

            RuleFor(r => r.FirstName).NotEmpty().WithMessage("First Name Is Required")
                .MinimumLength(3).MaximumLength(20).WithMessage("FirstName Must be Between 3 and 100 Character");

            RuleFor(r => r.LastName).NotEmpty().WithMessage("Last Name Is Required")
                .MinimumLength(3).MaximumLength(20).WithMessage("FirstName Must be Between 3 and 100 Character");

            RuleFor(r => r.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).MaximumLength(100).WithMessage("Password must be between 8 and 100 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character");

            RuleFor(r => r.ConfirmPassword).NotEmpty().WithMessage("Confirm Password is required")
                .Equal(r => r.Password).WithMessage("Password and confirmation password do not match");
            RuleFor(r => r.PhoneNumber).MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .Matches(@"^\+?[1-9]\d{1,14}$").When(r => !string.IsNullOrEmpty(r.PhoneNumber)).WithMessage("Invalid phone number format");
        }
    }
}