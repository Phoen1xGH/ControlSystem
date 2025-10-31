using ControlSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ControlSystem.Migrator.Initializers
{
    internal class DbConnectionBuilder
    {
        private const string _productionEnv = "Production";
        private const string _debugEnv = "Debug";

        public static DbContextOptions<ControlSystemContext> BuildDbConnection()
        {
            string? env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? _debugEnv;
            IConfiguration configuration = null;

            if (IsProductionEnvironment(env))
            {
                configuration = SecretsInitializer.InitializeProductionSecrets();
            }
            else
            {
                configuration = new ConfigurationBuilder()
                    .AddUserSecrets<Program>()
                    .Build();
            }

            if (configuration is null)
                throw new Exception("Configuration initialization error");

            IConfigurationSection dbSection = configuration.GetSection("DB")
                ?? throw new Exception("DB connection not found");

            IConfigurationSection mainDbSection = dbSection.GetSection("MAIN")
                ?? throw new Exception("DB connection not found");

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = mainDbSection["HOST"],
                Port = int.Parse(mainDbSection["PORT"]!),
                Database = mainDbSection["DB_NAME"],
                Username = mainDbSection["USER"],
                Password = mainDbSection["PASSWORD"],
                IncludeErrorDetail = false
            };

            return new DbContextOptionsBuilder<ControlSystemContext>()
                .UseNpgsql(builder.ConnectionString)
                .Options;
        }

        private static bool IsProductionEnvironment(string env)
            => env == _productionEnv;
    }
}
