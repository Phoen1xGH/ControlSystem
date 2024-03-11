namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Приоритет карточки
    /// </summary>
    public class Priority
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
        /// Цвет в hex-формате
        /// </summary>
        public string ColorHex { get; set; }
    }
}
