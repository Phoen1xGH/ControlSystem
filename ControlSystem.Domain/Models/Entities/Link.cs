namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Ссылка в карточке
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ресурс ссылки
        /// </summary>
        public string Source { get; set; }
    }
}
