namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Карточка, описывающая задачу
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Идентификатр в БД
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Создатель
        /// </summary>
        public UserAccount Author { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public UserAccount? Executor { get; set; }

        /// <summary>
        /// Участники
        /// </summary>
        public ICollection<UserAccount> Participants { get; set; } = new List<UserAccount>();


        /// <summary>
        /// Заголовок
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата крайнего срока выполнения
        /// </summary>
        public DateTime? DeadlineDate { get; set; }


        /// <summary>
        /// Файлы вложений
        /// </summary>
        public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();

        /// <summary>
        /// Ссылки
        /// </summary>
        public ICollection<Link> Links { get; set; } = new List<Link>();

        /// <summary>
        /// Комментарии
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();


        /// <summary>
        /// Приоритет
        /// </summary>
        public Priority? Priority { get; set; }

        /// <summary>
        /// Статус (доска, где находится карточка)
        /// </summary>
        public Board Status { get; set; }

        /// <summary>
        /// Теги
        /// </summary>
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
