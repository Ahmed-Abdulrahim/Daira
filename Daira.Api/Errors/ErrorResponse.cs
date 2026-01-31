namespace Daira.Api.Errors
{
    public class ErrorResponse
    {
        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public IDictionary<string, string[]>? Errors { get; set; }

        public string? TraceId { get; set; }
        public static ErrorResponse Create(string code, string message, string? traceId = null)
        {
            return new ErrorResponse
            {
                Code = code,
                Message = message,
                TraceId = traceId
            };
        }
        public static ErrorResponse ValidationError(IDictionary<string, string[]> errors, string? traceId = null)
        {
            return new ErrorResponse
            {
                Code = "VALIDATION_ERROR",
                Message = "One or more validation failures have occurred.",
                Errors = errors,
                TraceId = traceId
            };
        }
    }
}
