namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Тег карточки
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Идентификатор БД
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
