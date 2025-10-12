using Microsoft.Extensions.Configuration;

namespace ControlSystem.MainApp.Helpers
{
    public static class SecretsInitializer
    {
        const string _secretsDirectory = "/config/secrets/";

        public static IConfigurationBuilder InitializeProductionSecrets(this IConfigurationBuilder configuration) 
        {
            if (Directory.Exists(_secretsDirectory))
            {
                foreach (string secret in Directory.EnumerateFiles(_secretsDirectory, "*.json", SearchOption.AllDirectories))
                    configuration.AddJsonFile(secret);
            }

            return configuration;
        }
    }
}
