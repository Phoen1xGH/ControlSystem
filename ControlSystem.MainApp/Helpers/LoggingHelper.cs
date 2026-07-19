using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace ControlSystem.MainApp.Helpers
{
    public static class LoggingHelper
    {
        public static IApplicationBuilder UseStructuredRequestLogging(this IApplicationBuilder app)
        {
            // Прокидываем контекст запроса во ВСЕ логи внутри запроса (Enrich.FromLogContext).
            app.Use(async (context, next) =>
            {
                using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
                using (LogContext.PushProperty("RequestPath", context.Request.Path.Value))
                {
                    await next();
                }
            });

            // Один структурный лог на HTTP-запрос (метод, путь, статус, время).
            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = GetRequestLogLevel;
            });

            return app;
        }

        /// <summary>
        /// Уровень лога запроса: ошибки — Error, шумные /health и /metrics — Verbose, всё остальное — Information.
        /// </summary> 
        private static LogEventLevel GetRequestLogLevel(HttpContext httpContext, double elapsed, Exception? ex)
        {
            if (ex != null)
                return LogEventLevel.Error;

            var path = httpContext.Request.Path;
            if (path.StartsWithSegments("/health") || path.StartsWithSegments("/metrics"))
                return LogEventLevel.Verbose;

            return LogEventLevel.Information;
        }
    }
}
