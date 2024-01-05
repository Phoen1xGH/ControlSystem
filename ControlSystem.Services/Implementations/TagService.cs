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
    public class TagService : ITagService
    {
        private readonly ILogger<TagService> _logger;

        private IRepository<Tag> _tagRepository;

        private readonly IRepository<Ticket> _ticketRepository;

        public TagService(
            ILogger<TagService> logger,
            IRepository<Tag> tagRepository,
            IRepository<Ticket> ticketRepository)
        {
            _logger = logger;
            _tagRepository = tagRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<BaseResponse<bool>> CreateTag(Tag tag)
        {
            try
            {
                await (_tagRepository as TagsRepository)!.Create(tag);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateTag]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteTag(int tagId)
        {
            try
            {
                var tag = await _tagRepository.GetAll().FirstOrDefaultAsync(x => x.Id == tagId);

                if (tag == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.TagNotFound,
                        Description = StatusCode.TagNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_tagRepository as TagsRepository)!.Delete(tag);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteTag]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> EditTag(int tagId, Tag newTagData)
        {
            try
            {
                var tag = await _tagRepository.GetAll().FirstOrDefaultAsync(x => x.Id == tagId);

                if (tag == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.TagNotFound,
                        Description = StatusCode.TagNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                tag.Name = newTagData.Name;
                tag.ColorHex = newTagData.ColorHex;

                await _tagRepository.Update(tag);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditTag]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<Tag>> GetAllTags()
        {
            try
            {
                var tags = _tagRepository.GetAll().ToList();

                return new BaseResponse<List<Tag>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAllTags]: {ex.Message}");

                return new BaseResponse<List<Tag>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public BaseResponse<List<Tag>> GetTagsByTicket(int ticketId)
        {
            try
            {
                var ticket = _ticketRepository.GetAll().FirstOrDefault(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<List<Tag>>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                    };
                }

                var tags = ticket.Tags.ToList();

                return new BaseResponse<List<Tag>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tags
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetTagsByTicket]: {ex.Message}");

                return new BaseResponse<List<Tag>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<Tag>>> AddTagsToTicket(int ticketId, List<int> tagIds)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<List<Tag>>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                    };
                }

                var tags = _tagRepository.GetAll();

                foreach (var tagId in tagIds)
                {
                    var tag = tags.FirstOrDefault(t => t.Id == tagId);
                    if (tag != null)
                        ticket.Tags.Add(tag);
                }

                return new BaseResponse<List<Tag>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tags.ToList()
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddTagsToTicket]: {ex.Message}");

                return new BaseResponse<List<Tag>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Tag>> AddTagToTicket(int ticketId, int tagId)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<Tag>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                    };
                }

                var tag = await _tagRepository.GetAll().FirstOrDefaultAsync(x => x.Id == tagId);

                if (tag is null)
                {
                    return new BaseResponse<Tag>
                    {
                        StatusCode = StatusCode.TagNotFound,
                        Description = StatusCode.TagNotFound.GetDescriptionValue(),
                    };
                }

                ticket.Tags.Add(tag);

                await _ticketRepository.Update(ticket);

                return new BaseResponse<Tag>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tag
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddTagToTicket]: {ex.Message}");

                return new BaseResponse<Tag>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Tag>> RemoveTagFromTicket(int ticketId, int tagId)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<Tag>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                    };
                }

                var tag = await _tagRepository.GetAll().FirstOrDefaultAsync(x => x.Id == tagId);

                if (tag is null)
                {
                    return new BaseResponse<Tag>
                    {
                        StatusCode = StatusCode.TagNotFound,
                        Description = StatusCode.TagNotFound.GetDescriptionValue(),
                    };
                }

                ticket.Tags.Remove(tag);

                await _ticketRepository.Update(ticket);

                return new BaseResponse<Tag>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = tag
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[RemoveTagFromTicket]: {ex.Message}");

                return new BaseResponse<Tag>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
