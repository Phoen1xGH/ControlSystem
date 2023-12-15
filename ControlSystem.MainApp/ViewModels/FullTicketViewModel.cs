using ControlSystem.Domain.Entities;

namespace ControlSystem.MainApp.ViewModels
{
    public class FullTicketViewModel
    {
        public int Id { get; set; }

        public UserViewModel Author { get; set; }
        public UserViewModel? Executor { get; set; }
        public List<UserViewModel> Participants { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeadlineDate { get; set; }

        public List<FileViewModel>? Files { get; set; }
        public List<Link>? Links { get; set; }

        public List<CommentViewModel>? Comments { get; set; }

        public Priority? Priority { get; set; }

        public int StatusId { get; set; }

        public List<Tag> Tags { get; set; }


        public FullTicketViewModel(Ticket ticket)
        {
            var executor = ticket.Executor != null ?
                new UserViewModel(ticket.Executor)
                : null;

            var files = ticket.Attachments != null ?
                ticket.Attachments
                .Select(x => new FileViewModel(x)).ToList()
                : new List<FileViewModel>();

            var participants = ticket.Participants != null ?
                ticket.Participants
                      .Select(x => new UserViewModel(x)).ToList()
                      : new List<UserViewModel>();
            var comments = ticket.Comments != null ?
                ticket.Comments
                .Select(x => new CommentViewModel(x)).ToList()
                : new List<CommentViewModel>();

            Author = new UserViewModel(ticket.Author);
            Executor = executor;
            Participants = participants;
            Description = ticket.Description;
            Title = ticket.Title!;
            UpdatedDate = ticket.UpdatedDate;
            CreationDate = ticket.CreationDate;
            Files = files;
            Links = ticket.Links.ToList();
            Comments = comments;
            Priority = ticket.Priority;
            StatusId = ticket.Status.Id;
            Tags = ticket.Tags.ToList();
        }

    }
}
