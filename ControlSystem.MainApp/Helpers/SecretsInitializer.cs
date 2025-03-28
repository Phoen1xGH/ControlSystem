using Microsoft.Extensions.Configuration;

namespace ControlSystem.MainApp.Helpers
{
    public static class SecretsInitializer
    {
        const string _secretsDirectory = "/run/secrets/";

        public static IConfigurationBuilder InitializeDockerSecrets(this IConfigurationBuilder configuration) 
        {
            if (Directory.Exists(_secretsDirectory))
            {
                foreach (string secret in Directory.EnumerateFiles(_secretsDirectory, "*.json", SearchOption.AllDirectories))
                    configuration.AddJsonFile(secret);

                configuration.AddKeyPerFile(_secretsDirectory, false);
            }

            return configuration;
        }
    }
}
