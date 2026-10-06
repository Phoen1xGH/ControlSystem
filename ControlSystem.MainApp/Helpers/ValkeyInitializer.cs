using ControlSystem.MainApp.Options;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace ControlSystem.MainApp.Helpers
{
    public static class ValkeyInitializer
    {
        public static async Task InitializeValkeyConnectionAsync(this WebApplicationBuilder builder)
        {
            var valkeyOptions = builder.Configuration
                .GetSection(ProjectValkeyOptions.Section)
                .Get<ProjectValkeyOptions>();

            if (valkeyOptions == null || !valkeyOptions.IsValid())
                throw new ArgumentNullException("Отсутствуют данные для подключения к Valkey");

            var options = new ConfigurationOptions
            {
                EndPoints = { $"{valkeyOptions.HOST}:{valkeyOptions.PORT}" },
                Password = string.IsNullOrEmpty(valkeyOptions.PASSWORD) ? string.Empty : valkeyOptions.PASSWORD,
                AbortOnConnectFail = false,
                ConnectRetry = valkeyOptions.RETRY,
                ConnectTimeout = valkeyOptions.TIMEOUT
            };

            var valkey = await ConnectionMultiplexer.ConnectAsync(options);

            builder.Services.AddSingleton<IConnectionMultiplexer>(valkey);

            builder.Services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(valkey, "DataProtection-Keys")
                .SetApplicationName("TimeSenseWorkflow");
        }
    }
}
