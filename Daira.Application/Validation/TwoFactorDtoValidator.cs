namespace Daira.Application.Validation
{
    public class TwoFactorDtoValidator : AbstractValidator<TwoFactorDto>
    {
        public TwoFactorDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .InclusiveBetween(100000, 999999).WithMessage("Code must be a 6-digit number.");
        }
    }
}
