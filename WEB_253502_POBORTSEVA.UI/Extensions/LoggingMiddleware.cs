namespace WEB_253502_POBORTSEVA.UI.Extensions
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                var requestPath = context.Request.Path;
                var statusCode = context.Response.StatusCode;
                _logger.LogInformation($"---> request {requestPath} returns {statusCode}");
            }
        }
    }
}
