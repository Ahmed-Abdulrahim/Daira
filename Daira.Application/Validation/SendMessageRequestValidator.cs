namespace Daira.Application.Validation
{
    public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidator()
        {
            RuleFor(s => s.ConversationId).NotEmpty().WithMessage("Conversation Id Is Required");
            RuleFor(s => s.Content).NotEmpty().WithMessage("Content is Required")
                .MaximumLength(4000).WithMessage("Message content must be between 1 and 4000 characters");
        }
    }
}
