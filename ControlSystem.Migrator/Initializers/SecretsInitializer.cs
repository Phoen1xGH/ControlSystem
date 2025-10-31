using Microsoft.Extensions.Configuration;

namespace ControlSystem.Migrator.Initializers
{
    internal static class SecretsInitializer
    {
        const string _secretsDirectory = "/config/secrets/";

        public static IConfiguration InitializeProductionSecrets()
        {
            var configurationBuilder = new ConfigurationBuilder();

            if (Directory.Exists(_secretsDirectory))
            {
                foreach (string secret in Directory.EnumerateFiles(_secretsDirectory, "*.json", SearchOption.AllDirectories))
                    configurationBuilder.AddJsonFile(secret, optional: false, reloadOnChange: false);
            }

            return configurationBuilder.Build();
        }
    }
}
