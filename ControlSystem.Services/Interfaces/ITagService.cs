using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;

namespace ControlSystem.Services.Interfaces
{
    public interface ITagService
    {
        Task<BaseResponse<bool>> CreateTag(Tag tag);

        Task<BaseResponse<bool>> DeleteTag(int tagId);

        Task<BaseResponse<bool>> EditTag(int tagId, Tag newTagData);

        BaseResponse<List<Tag>> GetAllTags();

        BaseResponse<List<Tag>> GetTagsByTicket(int ticketId);

        Task<BaseResponse<List<Tag>>> AddTagsToTicket(int ticketId, List<int> tagIds);

        Task<BaseResponse<Tag>> AddTagToTicket(int ticketId, int tagId);

        Task<BaseResponse<Tag>> RemoveTagFromTicket(int ticketId, int tagId);
    }
}
