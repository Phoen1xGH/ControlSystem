using ControlSystem.MainApp.Options;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace ControlSystem.MainApp.Helpers
{
    public static class RedisInitializer
    {
        public static async Task InitializeRedisConnectionAsync(this WebApplicationBuilder builder)
        {
            var redisOptions = builder.Configuration
                .GetSection(ProjectRedisOptions.Section)
                .Get<ProjectRedisOptions>();

            if (redisOptions == null || !redisOptions.IsValid())
                throw new ArgumentNullException("Отсутствуют данные для подключения к Redis");

            var options = new ConfigurationOptions
            {
                EndPoints = { $"{redisOptions.HOST}:{redisOptions.PORT}" },
                Password = string.IsNullOrEmpty(redisOptions.PASSWORD) ? string.Empty : redisOptions.PASSWORD,
                AbortOnConnectFail = false,
                ConnectRetry = redisOptions.RETRY,
                ConnectTimeout = redisOptions.TIMEOUT
            };

            var redis = await ConnectionMultiplexer.ConnectAsync(options);

            builder.Services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys")
                .SetApplicationName("TimeSenseWorkflow");
        }
    }
}
