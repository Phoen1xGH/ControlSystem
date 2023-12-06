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
    }
}
