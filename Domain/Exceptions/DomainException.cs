namespace Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }

        public DomainException(string message, string errorCode = "DomainError")
            : base(message)
        {
            ErrorCode = errorCode;
        }
        public DomainException(string message, Exception innerException, string errorCode = "DomainError")
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
