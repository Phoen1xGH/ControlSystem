namespace ControlSystem.Domain.Entities
{
    /// <summary>
    /// Аккаунт пользователя
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Захэшированный пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// email-адрес
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Диаграммы бизнес-процессов
        /// </summary>
        public ICollection<Chart> Charts { get; set; } = new List<Chart>();

        /// <summary>
        /// Рабочие пространства
        /// </summary>
        public ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();
    }
}
