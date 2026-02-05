namespace Daira.Application.Validation.NotificationModule
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Type)
                .NotNull()
                .IsInEnum();

            RuleFor(x => x)
                .Must(BeValidNotificationStructure)
                .WithMessage("Invalid notification structure based on type");

            RuleFor(x => x.content)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.content));
        }

        private bool BeValidNotificationStructure(CreateNotificationDto dto)
        {
            return dto.Type switch
            {
                NotificationType.Like =>
                    dto.targetId.HasValue && dto.targetId != Guid.Empty,

                NotificationType.Comment =>
                    dto.targetId.HasValue && dto.targetId != Guid.Empty,

                NotificationType.Follow =>
                    !string.IsNullOrEmpty(dto.ActorId),

                NotificationType.FriendRequest =>
                    !string.IsNullOrEmpty(dto.ActorId),

                NotificationType.Message =>
                    dto.targetId.HasValue && dto.targetId != Guid.Empty &&
                    !string.IsNullOrEmpty(dto.ActorId),

                _ => true
            };
        }
    }
}
