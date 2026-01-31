namespace Daira.Application.Validation
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is Required")
                .EmailAddress().WithMessage("Invalid Email Address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required");
        }
    }
}
