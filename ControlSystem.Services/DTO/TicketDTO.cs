using ControlSystem.Domain.Entities;

namespace ControlSystem.Services.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }

        public UserDTO Author { get; set; }
        public UserDTO? Executor { get; set; }
        public List<UserDTO>? Participants { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeadlineDate { get; set; }

        public List<FileDTO>? Files { get; set; }
        public List<Link>? Links { get; set; }

        public List<CommentDTO>? Comments { get; set; }

        public Priority? Priority { get; set; }

        public int StatusId { get; set; }

        public List<Tag>? Tags { get; set; }

        public List<BoardDTO>? Statuses { get; set; }
    }
}
