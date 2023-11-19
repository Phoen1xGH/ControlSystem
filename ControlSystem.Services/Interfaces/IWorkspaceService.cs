using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;

namespace ControlSystem.Services.Interfaces
{
    public interface IWorkspaceService
    {
        Task<BaseResponse<bool>> CreateWorkspace(string username, string workspaceName);

        Task<BaseResponse<bool>> RenameWorkspace(int id, string workspaceName);

        Task<BaseResponse<bool>> CreateBoard(int id, Board board);
        Task<BaseResponse<bool>> DeleteWorkspace(string username, int workspaceId);

        BaseResponse<List<Workspace>> GetWorkspaces(string username);
    }
}
