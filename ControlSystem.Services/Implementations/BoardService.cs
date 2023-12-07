using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class BoardService : IBoardService
    {
        private readonly ILogger<BoardService> _logger;

        private readonly IRepository<UserAccount> _userRepository;

        private readonly IRepository<Board> _boardRepository;

        private readonly IRepository<Ticket> _ticketRepository;

        public BoardService(ILogger<BoardService> logger,
            IRepository<UserAccount> userRepository,
            IRepository<Board> boardRepository,
            IRepository<Ticket> ticketRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<BaseResponse<int>> CreateTicket(string username, Ticket ticket)
        {
            try
            {
                var board = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticket.Status.Id);

                if (board is null)
                {
                    return new BaseResponse<int>
                    {
                        StatusCode = StatusCode.BoardNotFound,
                        Description = StatusCode.BoardNotFound.GetDescriptionValue(),
                        Data = -1
                    };
                }

                var author = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                var participants = ticket.Participants is not null ? ticket.Participants : board.Workspace.Participants;

                ticket.Participants = participants;

                await (_boardRepository as BoardRepository)!.AddTicket(board, ticket);

                return new BaseResponse<int>
                {
                    StatusCode = StatusCode.OK,
                    Data = board.Id,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateTicket]: {ex.Message}");

                return new BaseResponse<int>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = -1
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteTicket(int ticketId)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_ticketRepository as TicketRepository)!.Delete(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteTicket]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<Ticket>> GetTickets(int boardId)
        {
            try
            {
                var tickets = _boardRepository.GetAll().FirstOrDefault(x => x.Id == boardId)!.Tickets.ToList();

                return new BaseResponse<List<Ticket>>()
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tickets
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetTickets]: {ex.Message}");

                return new BaseResponse<List<Ticket>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                };
            }
        }

        public async Task<BaseResponse<bool>> EditTicket(int ticketId, Ticket newTicketData)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                ticket.Status = newTicketData.Status;
                ticket.Author = newTicketData.Author;
                ticket.Description = newTicketData.Description;
                ticket.Priority = newTicketData.Priority;
                ticket.Attachments = newTicketData.Attachments;
                ticket.Participants = newTicketData.Participants;
                ticket.Comments = newTicketData.Comments;
                ticket.Tags = newTicketData.Tags;
                ticket.Links = newTicketData.Links;
                ticket.DeadlineDate = newTicketData.DeadlineDate;
                ticket.Executor = newTicketData.Executor;
                ticket.Title = newTicketData.Title;

                await (_ticketRepository as TicketRepository)!.Update(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditTicket]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }
    }
}
