namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Комментарий в карточке
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Создатель комментария
        /// </summary>
        public UserAccount Author { get; set; }

        /// <summary>
        /// Содержимое комментария
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
