namespace ControlSystem.MainApp.Options
{
    public class ProjectRedisOptions
    {
        public const string Section = "REDIS";

        public string HOST { get; set; } = string.Empty;

        public int PORT { get; set; } = 6379;

        public int RETRY { get; set; } = 3;

        public int TIMEOUT { get; set; } = 5000;

        public string PASSWORD { get; set; } = string.Empty;

        /// <summary>
        /// Проверка, что данные указанны правильно
        /// </summary>
        /// <returns>True - данные указанны правильно, иначе false</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(HOST);
        }
    }
}
