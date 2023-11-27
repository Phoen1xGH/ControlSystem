using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;

namespace ControlSystem.Services.Interfaces
{
    public interface IBoardService
    {
        Task<BaseResponse<int>> CreateTicket(string username, Ticket ticket);

        Task<BaseResponse<bool>> RenameTicket(int ticketId, string newTitle);

        Task<BaseResponse<bool>> DeleteTicket(int ticketId);

        BaseResponse<List<Ticket>> GetTickets(int boardId);
    }
}
