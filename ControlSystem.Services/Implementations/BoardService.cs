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
        private readonly ILogger<UserAccountService> _logger;

        private readonly IRepository<UserAccount> _userRepository;

        private readonly IRepository<Board> _boardRepository;

        private readonly IRepository<Ticket> _ticketRepository;

        public BoardService(ILogger<UserAccountService> logger,
            IRepository<UserAccount> userRepository,
            IRepository<Board> boardRepository,
            IRepository<Ticket> ticketRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<BaseResponse<bool>> CreateTicket(string username, int boardId, string title)
        {
            try
            {
                var board = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == boardId);

                if (board is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.BoardNotFound,
                        Description = StatusCode.BoardNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                var author = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                var ticket = new Ticket
                {
                    Title = title,
                    Author = author!,
                    ShareLink = new() { Name = "", Source =$"/Workspaces/{board.Workspace.Id}/card{board.Tickets.Count + 1}" },
                    Status = board
                };

                await (_boardRepository as BoardRepository)!.AddTicket(board, ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateTicket]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public Task<BaseResponse<bool>> DeleteTicket(int ticketId)
        {
            throw new NotImplementedException();
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

        public Task<BaseResponse<bool>> RenameTicket(int ticketId, string newTitle)
        {
            throw new NotImplementedException();
        }
    }
}
