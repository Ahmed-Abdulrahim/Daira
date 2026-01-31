namespace Domain.Exceptions
{
    public class ValidationException : DomainException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.", "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation failures have occurred.", "VALIDATION_ERROR")
        {
            Errors = errors;
        }
        public ValidationException(string propertyName, string errorMessage)
            : base($"Validation failed for {propertyName}: {errorMessage}", "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        };
        }
    }
}
