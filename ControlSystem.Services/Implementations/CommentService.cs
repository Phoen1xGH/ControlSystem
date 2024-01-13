using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Services.DTO;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ILogger<CommentService> _logger;

        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<UserAccount> _userRepository;

        public CommentService(ILogger<CommentService> logger,
                              IRepository<Comment> commentRepository,
                              IRepository<Ticket> ticketReposirory,
                              IRepository<UserAccount> userRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _ticketRepository = ticketReposirory;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<CommentDTO>> CreateComment(int ticketId, string author, string content)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<CommentDTO>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == author);

                if (user is null)
                {
                    return new BaseResponse<CommentDTO>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                var time = DateTime.Now;

                ticket.Comments.Add(new Comment { Author = user, Content = content, CreationDate = time });

                await _ticketRepository.Update(ticket);

                return new BaseResponse<CommentDTO>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = new CommentDTO { AuthorName = author, Content = content, CreationDate = time.ToString("dd.MM.yyyy  HH:mm") }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateComment]: {ex.Message}");

                return new BaseResponse<CommentDTO>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteComment(int commentId)
        {
            try
            {
                var comment = await _commentRepository.GetAll().FirstOrDefaultAsync(x => x.Id == commentId);

                if (comment is null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = StatusCode.CommentNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _commentRepository.Delete(comment);

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteComment]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<CommentDTO>> EditComment(int commentId, string newContent)
        {
            try
            {
                var comment = await _commentRepository.GetAll().FirstOrDefaultAsync(x => x.Id == commentId);

                if (comment is null)
                {
                    return new BaseResponse<CommentDTO>()
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = StatusCode.CommentNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                comment.Content = newContent;

                await _commentRepository.Update(comment);

                return new BaseResponse<CommentDTO>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = new CommentDTO
                    {
                        AuthorName = comment.Author.Username,
                        Content = comment.Content,
                        CreationDate = comment.CreationDate.ToString("dd.MM.yyyy  HH:mm")
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditComment]: {ex.Message}");

                return new BaseResponse<CommentDTO>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
