namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Информация об обновлениях
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Краткое содержание
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Полное описание
        /// </summary>
        public string Description { get; set; }
    }
}
