namespace ControlSystem.MainApp.Options
{
    public class ProjectDbOptions
    {
        public const string Section = "DB";
        public DbOptions? MAIN { get; set; }

        /// <summary>
        /// Проверка, что данные указанны правильно
        /// </summary>
        /// <returns>True - данные указанны правильно, иначе false</returns>
        public bool IsValid()
        {
            return MAIN != null && MAIN.IsValid();
        }
    }
}
