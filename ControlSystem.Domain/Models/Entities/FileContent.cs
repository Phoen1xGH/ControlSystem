namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Содержимое файла
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Содержание
        /// </summary>
        public byte[] Content { get; set; }
    }
}
