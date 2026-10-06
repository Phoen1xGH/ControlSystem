using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace ControlSystem.MainApp.HealthChecks
{
    public class ValkeyHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _valkey;

        public ValkeyHealthCheck(IConnectionMultiplexer valkey)
        {
            _valkey = valkey;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var db = _valkey.GetDatabase();
                var pong = await db.PingAsync();
                return HealthCheckResult.Healthy("Valkey is OK");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Valkey is down", ex);
            }
        }
    }
}
