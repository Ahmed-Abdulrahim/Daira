namespace Daira.Application.Validation.ConversationModule
{
    public class CreateConversationRequestCValidator : AbstractValidator<CreateConversationRequest>
    {
        public CreateConversationRequestCValidator()
        {
            RuleFor(c => c.Type)
            .NotEmpty().WithMessage("Type is required")
            .Must(type => type == "Direct" || type == "Group")
            .WithMessage("Type must be either 'Direct' or 'Group'");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required")
                .When(c => c.Type == "Group");

            RuleFor(c => c.ParticipantIds)
                .NotEmpty().WithMessage("At least one participant is required")
                .Must(list => list != null && list.Count > 0)
                .WithMessage("Participant list cannot be empty");

            RuleFor(c => c.ParticipantIds)
                .Must(list => list.Count == 1)
                .WithMessage("Direct conversation must have exactly one participant")
                .When(c => c.Type == "Direct");

            RuleFor(c => c.ParticipantIds)
                .Must(list => list.Count >= 2)
                .WithMessage("Group conversation must have at least 2 participants")
                .When(c => c.Type == "Group");


        }
    }
}
