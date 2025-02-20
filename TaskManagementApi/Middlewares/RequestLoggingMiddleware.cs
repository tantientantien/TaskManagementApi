using System.Diagnostics;

namespace TaskManagementApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode;
                var elapsedTime = stopwatch.ElapsedMilliseconds;

                _logger.LogInformation("[{Timestamp}] {Method} {Path} => {StatusCode} ({ElapsedTime}ms)",
                    timestamp, method, path, statusCode, elapsedTime);
            }
        }
    }
}