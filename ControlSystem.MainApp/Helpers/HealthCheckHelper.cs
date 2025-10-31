using ControlSystem.DAL;
using ControlSystem.MainApp.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace ControlSystem.MainApp.Helpers
{
    public static class HealthCheckHelper
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ControlSystemContext>("postgres")
                .AddCheck<RedisHealthCheck>("redis");

            return services;
        }

        public static IEndpointRouteBuilder MapHealthCheck(this IEndpointRouteBuilder app)
        {
            // Liveness — проверяет, что приложение живо
            app.MapHealthChecks("/health/live");

            // Readiness — проверяет, что приложение готово принимать трафик
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = _ => true, // проверять все зарегистрированные HealthChecks
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var result = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new { e.Key, e.Value.Status, e.Value.Description })
                    });

                    await context.Response.WriteAsync(result);
                }
            });

            return app;
        }
    }
}
