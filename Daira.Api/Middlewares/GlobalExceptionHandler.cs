namespace Daira.Api.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = context.TraceIdentifier;

            var (statusCode, response) = exception switch
            {
                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    ErrorResponse.ValidationError(validationEx.Errors, traceId)),

                AuthenticationException authEx => (
                    HttpStatusCode.Unauthorized,
                    ErrorResponse.Create(authEx.ErrorCode, authEx.Message, traceId)),

                NotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    ErrorResponse.Create("NOT_FOUND", notFoundEx.Message, traceId)),

                DomainException domainEx => (
                    HttpStatusCode.BadRequest,
                    ErrorResponse.Create(domainEx.ErrorCode, domainEx.Message, traceId)),

                UnauthorizedAccessException => (
                    HttpStatusCode.Unauthorized,
                    ErrorResponse.Create("UNAUTHORIZED", "Access denied.", traceId)),

                _ => (
                    HttpStatusCode.InternalServerError,
                    ErrorResponse.Create("INTERNAL_ERROR", "An unexpected error occurred.", traceId))
            };

            _logger.LogError(exception,
                "Exception occurred. TraceId: {TraceId}, StatusCode: {StatusCode}, Message: {Message}",
                traceId, (int)statusCode, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }

    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandler>();
        }
    }
}
