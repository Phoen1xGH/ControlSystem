namespace ControlSystem.Domain.Entities
{
    public class FileAttachment
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public FileContent FileContent { get; set; }

    }
}
