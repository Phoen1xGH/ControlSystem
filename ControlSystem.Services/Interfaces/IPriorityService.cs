using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;

namespace ControlSystem.Services.Interfaces
{
    public interface IPriorityService
    {
        Task<BaseResponse<bool>> CreatePriority(Priority priority);
        Task<BaseResponse<bool>> EditPriority(int priorityId, Priority newDataPriority);
        Task<BaseResponse<bool>> DeletePriority(int priorityId);
        BaseResponse<List<Priority>> GetPriorities();

    }
}
