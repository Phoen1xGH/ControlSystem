namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Файловые вложения
    /// </summary>
    public class FileAttachment
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Содержание файла
        /// </summary>
        public FileContent FileContent { get; set; }
    }
}
