namespace ControlSystem.Domain.Entities
{
    public class Workspace
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<UserAccount>? Participants { get; set; }
        public ICollection<Board>? Boards { get; set; }
    }
}
