using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using ControlSystem.Services.DTO;
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

        private readonly IRepository<Comment> _commentRepository;

        private readonly IRepository<FileAttachment> _fileRepository;

        public BoardService(ILogger<BoardService> logger,
            IRepository<UserAccount> userRepository,
            IRepository<Board> boardRepository,
            IRepository<Ticket> ticketRepository,
            IRepository<Comment> commentRepository,
            IRepository<FileAttachment> fileRepostory)
        {
            _logger = logger;
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _ticketRepository = ticketRepository;
            _commentRepository = commentRepository;
            _fileRepository = fileRepostory;
        }

        public async Task<BaseResponse<int>> CreateTicket(string username, TicketViewModel ticketVM)
        {
            try
            {
                var board = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketVM.StatusId);

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

                var participants = ticketVM.Participants is not null ? ticketVM.Participants : board.Workspace.Participants;

                var ticket = new Ticket
                {
                    Author = author!,
                    Executor = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == ticketVM.ExecutorName),
                    Participants = participants,
                    Title = ticketVM.Title,
                    Description = ticketVM.Description,
                    CreationDate = ticketVM.CreationDate,
                    UpdatedDate = ticketVM.UpdatedDate,
                    DeadlineDate = ticketVM.DeadlineDate,
                    Attachments = ticketVM.Attachments,
                    Links = ticketVM.Links,
                    Comments = ticketVM.Comments,
                    Priority = ticketVM.Priority,
                    Status = board,
                    Tags = ticketVM.Tags,

                };

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

        public async Task<BaseResponse<int>> CreateTicket(string username, string title, int boardId)
        {
            try
            {
                var board = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == boardId);

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

                if (author is null)
                {
                    return new BaseResponse<int>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = -1
                    };
                }

                var ticket = new Ticket { Title = title, Author = author };

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
                var tickets = _boardRepository.GetAll()
                    .FirstOrDefault(x => x.Id == boardId)!
                    .Tickets
                    .OrderBy(x => x.CreationDate)
                    .ToList();

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

        public async Task<BaseResponse<bool>> EditTicket(int ticketId, TicketChangesDTO newTicketData)
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

                //var status = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == newTicketData.StatusId);
                //var author = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == newTicketData.Author.Name);

                //var executor = newTicketData.Executor is not null ?
                //    await _userRepository.GetAll()
                //        .FirstOrDefaultAsync(x => x.Username == newTicketData.Executor.Name) :
                //    null;

                //var users = _userRepository.GetAll();
                //var commentsAll = _commentRepository.GetAll();
                //var filesAll = _fileRepository.GetAll();

                //var participants = newTicketData.Participants is not null ? users
                //    .Where(user => newTicketData.Participants
                //        .Any(part => part.Name == user.Username))
                //    .ToList() :
                //    null;

                //var files = newTicketData.Files is not null ?
                //    filesAll
                //        .Where(file => newTicketData.Files
                //            .Any(fileDTO => fileDTO.Id == file.Id))
                //        .ToList() :
                //    new();

                //var comments = newTicketData.Comments is not null ?
                //    commentsAll
                //        .Where(comment => newTicketData.Comments
                //            .Any(commentDTO => commentDTO.Id == comment.Id))
                //        .ToList() :
                //    new();

                //ticket.Status = status!;
                //ticket.Author = author!;
                //ticket.Description = newTicketData.Description;
                //ticket.Priority = newTicketData.Priority;
                //ticket.Attachments = files;
                //ticket.Participants = participants;
                //ticket.Comments = comments;
                //ticket.Tags = newTicketData.Tags;
                //ticket.Links = newTicketData.Links;
                //ticket.DeadlineDate = newTicketData.DeadlineDate;
                //ticket.Executor = executor;
                //ticket.Title = newTicketData.Title;
                //ticket.UpdatedDate = DateTime.Now;

                ticket.Description = newTicketData.Description;
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

        public async Task<BaseResponse<bool>> AddTicketPriority(int ticketId, Priority priority)
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

                ticket.Priority = priority;

                await _ticketRepository.Update(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddTicketPriority]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> AddTicketTags(int ticketId, List<Tag> tags)
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

                ticket.Tags = tags;

                await _ticketRepository.Update(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddTicketTags]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }


        public async Task<BaseResponse<bool>> AddTicketLink(int ticketId, Link link)
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

                ticket.Links.Add(link);

                await _ticketRepository.Update(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddTicketLink]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> AddTicketComment(int ticketId, CommentDTO comment)
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

                var author = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Username == comment.AuthorName);

                if (author is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                ticket.Comments.Add(new Comment { Content = comment.Content, Author = author });

                await _ticketRepository.Update(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddTicketComment]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<Ticket>> GetTicketById(int ticketId)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<Ticket>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                return new BaseResponse<Ticket>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = ticket
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetTicketById]: {ex.Message}");

                return new BaseResponse<Ticket>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
