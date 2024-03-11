namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Доска
    /// </summary>
    public class Board
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

        /// <summary>
        /// Пространство, в котором находится доска
        /// </summary>
        public Workspace Workspace { get; set; }

        /// <summary>
        /// Список карточек
        /// </summary>
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}