namespace ControlSystem.Domain.Entities
{
    public class UpdateInfo
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Version { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
    }
}
