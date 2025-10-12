namespace ControlSystem.MainApp.Helpers
{
    public static class ConnectionHelper
    {
        public static string GetDbConnectionString(this IWebHostEnvironment environment)
        {
            return environment.IsDockerEnvironment() ?
                "DockerDatabase" : "DefaultDatabase";
        }

        private static bool IsDockerEnvironment(this IWebHostEnvironment environment)
            => environment.IsEnvironment("Docker");
        
    }
}
