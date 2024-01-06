using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class LinkService : ILinkService
    {
        private readonly ILogger<LinkService> _logger;

        private readonly IRepository<Link> _linkRepository;
        private readonly IRepository<Ticket> _ticketRepository;

        public LinkService(ILogger<LinkService> logger,
            IRepository<Link> linkRepository,
            IRepository<Ticket> ticketRepository)
        {
            _logger = logger;
            _linkRepository = linkRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<BaseResponse<bool>> CreateLink(int ticketId, Link link)
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
                _logger.LogError(ex, $"[CreateLink]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteLink(int linkId)
        {
            try
            {
                var link = await _linkRepository.GetAll().FirstOrDefaultAsync(x => x.Id == linkId);

                if (link is null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.LinkNotFound,
                        Description = StatusCode.LinkNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _linkRepository.Delete(link);

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteLink]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> EditLink(int linkId, Link newLinkData)
        {
            try
            {
                var link = await _linkRepository.GetAll().FirstOrDefaultAsync(x => x.Id == linkId);

                if (link is null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.LinkNotFound,
                        Description = StatusCode.LinkNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                link.Source = newLinkData.Source;
                link.Name = newLinkData.Name;

                await _linkRepository.Update(link);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditLink]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<Link>> GetLinks()
        {
            try
            {
                var links = _linkRepository.GetAll().ToList();

                return new BaseResponse<List<Link>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = links

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetLinks]: {ex.Message}");

                return new BaseResponse<List<Link>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<Link>>> GetLinksByTicket(int ticketId)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<List<Link>>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                return new BaseResponse<List<Link>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = ticket.Links.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetLinksByTicket]: {ex.Message}");

                return new BaseResponse<List<Link>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
