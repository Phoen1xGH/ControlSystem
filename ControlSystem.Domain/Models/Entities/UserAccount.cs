namespace ControlSystem.Domain.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public ICollection<Chart> Charts { get; set; } = new List<Chart>();
        public ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();

    }
}
