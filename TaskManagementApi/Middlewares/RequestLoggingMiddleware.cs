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

                if (elapsedTime > 500)
                {
                    _logger.LogWarning("{Method} {Path} took {ElapsedTime}ms => {StatusCode}",
                        method, path, elapsedTime, statusCode);
                }
                else if (statusCode >= 400)
                {
                    _logger.LogError("{Method} {Path} => {StatusCode} ({ElapsedTime}ms)",
                        method, path, statusCode, elapsedTime);
                }
            }
        }
    }
}