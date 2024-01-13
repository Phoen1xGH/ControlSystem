using ControlSystem.Domain.Response;
using ControlSystem.Services.DTO;

namespace ControlSystem.Services.Interfaces
{
    public interface ICommentService
    {
        Task<BaseResponse<CommentDTO>> CreateComment(int ticketId, string author, string content);
    }
}
