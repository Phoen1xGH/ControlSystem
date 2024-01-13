namespace ControlSystem.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public UserAccount Author { get; set; }
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
