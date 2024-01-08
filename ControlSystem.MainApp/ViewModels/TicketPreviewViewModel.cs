using ControlSystem.Domain.Entities;

namespace ControlSystem.MainApp.DTO
{
    public class TicketPreviewViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int WorkspaceId { get; set; }
        public int StatusId { get; set; }

        public Priority? Priority { get; set; }
    }
}
