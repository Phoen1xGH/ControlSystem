using ControlSystem.Domain.Entities;

namespace ControlSystem.MainApp.ViewModels
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FileViewModel(FileAttachment fileAttachment)
        {
            Id = fileAttachment.Id;
            Name = fileAttachment.FileName;
        }
    }
}
