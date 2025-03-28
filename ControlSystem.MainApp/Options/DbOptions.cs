using Microsoft.Extensions.Hosting;

namespace ControlSystem.MainApp.Options
{
    public class DbOptions
    {
        public string HOST { get; set; } = string.Empty;

        public int PORT { get; set; }

        public string DB_NAME { get; set; } = string.Empty;

        public string USER { get; set; } = string.Empty;

        public string PASSWORD { get; set; } = string.Empty;

        public bool ERRORS { get; set; }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(HOST) &&
                !string.IsNullOrEmpty(DB_NAME) &&
                !string.IsNullOrEmpty(USER) &&
                !string.IsNullOrEmpty(PASSWORD);
        }
    }
}
