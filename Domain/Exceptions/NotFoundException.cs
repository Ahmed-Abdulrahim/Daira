namespace Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string message)
       : base(message, "NOT_FOUND")
        {
        }

        public NotFoundException(string entityName, object key)
            : base($"Entity \"{entityName}\" ({key}) was not found.", "NOT_FOUND")
        {
        }
        public static NotFoundException UserNotFound(string identifier)
            => new("User", identifier);
    }
}
