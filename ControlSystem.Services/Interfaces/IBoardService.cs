using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;

namespace ControlSystem.Services.Interfaces
{
    public interface IBoardService
    {
        Task<BaseResponse<int>> CreateTicket(string username, TicketViewModel ticketVM);

        Task<BaseResponse<int>> CreateTicket(string username, string title, int boardId);

        Task<BaseResponse<bool>> EditTicket(int ticketId, TicketViewModel newTicketData);

        Task<BaseResponse<bool>> DeleteTicket(int ticketId);

        BaseResponse<List<Ticket>> GetTickets(int boardId);
    }
}
