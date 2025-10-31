using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace ControlSystem.MainApp.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisHealthCheck(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var db = _redis.GetDatabase();
                var pong = await db.PingAsync();
                return HealthCheckResult.Healthy("Redis is OK");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Redis is down", ex);
            }
        }
    }
}
