using ControlSystem.Domain.Entities;

namespace ControlSystem.Domain.ViewModels
{
    public class TicketViewModel
    {
        public string? AuthorName { get; set; }
        public string? ExecutorName { get; set; }
        public ICollection<UserAccount> Participants { get; set; } = new List<UserAccount>();

        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateTime? DeadlineDate { get; set; }

        public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
        public ICollection<Link> Links { get; set; } = new List<Link>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Priority? Priority { get; set; }
        public int StatusId { get; set; }
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
