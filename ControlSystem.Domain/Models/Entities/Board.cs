namespace ControlSystem.Domain.Entities
{
    public class Board
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string ColorHex { get; set; }

        public Workspace Workspace { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}