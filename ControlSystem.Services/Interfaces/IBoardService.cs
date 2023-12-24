using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using ControlSystem.Services.DTO;

namespace ControlSystem.Services.Interfaces
{
    public interface IBoardService
    {
        Task<BaseResponse<int>> CreateTicket(string username, TicketViewModel ticketVM);

        Task<BaseResponse<int>> CreateTicket(string username, string title, int boardId);

        Task<BaseResponse<bool>> EditTicket(int ticketId, TicketDTO newTicketData);

        Task<BaseResponse<bool>> DeleteTicket(int ticketId);

        BaseResponse<List<Ticket>> GetTickets(int boardId);
        Task<BaseResponse<Ticket>> GetTicketById(int ticketId);
    }
}
