using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;

namespace ControlSystem.Services.Interfaces
{
    public interface IUpdatesService
    {
        Task<BaseResponse<bool>> AddUpdateInfo(string version, string topic, string description);

        Task<BaseResponse<bool>> DeleteUpdateInfo(int updateId);

        BaseResponse<List<UpdateInfo>> GetUpdatesInfo();
    }
}
