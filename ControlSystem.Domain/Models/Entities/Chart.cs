namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Диаграмма бизнес-процесса
    /// </summary>
    public class Chart
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок (наименование)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Содержимое в виде xml-таблицы
        /// </summary>
        public string XmlData { get; set; }
    }
}
