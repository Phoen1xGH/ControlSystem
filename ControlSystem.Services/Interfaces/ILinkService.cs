using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;

namespace ControlSystem.Services.Interfaces
{
    public interface ILinkService
    {
        BaseResponse<List<Link>> GetLinks();

        Task<BaseResponse<List<Link>>> GetLinksByTicket(int ticketId);

        Task<BaseResponse<bool>> CreateLink(int ticketId, Link link);

        Task<BaseResponse<bool>> DeleteLink(int linkId);

        Task<BaseResponse<bool>> EditLink(int linkId, Link newLinkData);
    }
}
