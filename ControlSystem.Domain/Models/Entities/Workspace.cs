namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Рабочее пространство
    /// </summary>
    public class Workspace
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
        /// Участники
        /// </summary>
        public ICollection<UserAccount> Participants { get; set; } = new List<UserAccount>();

        /// <summary>
        /// Доски
        /// </summary>
        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }
}
