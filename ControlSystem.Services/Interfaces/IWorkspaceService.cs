using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;

namespace ControlSystem.Services.Interfaces
{
    public interface IWorkspaceService
    {
        Task<BaseResponse<bool>> CreateWorkspace(string username, string workspaceName);

        Task<BaseResponse<bool>> RenameWorkspace(int id, string workspaceName);

        Task<BaseResponse<bool>> CreateBoard(int id, BoardViewModel board);

        Task<BaseResponse<bool>> DeleteWorkspace(string username, int workspaceId);

        BaseResponse<List<Workspace>> GetWorkspaces(string username);

        BaseResponse<List<Board>> GetBoards(int workspaceId);

        Task<BaseResponse<int>> EditBoard(int id, BoardViewModel boardViewModel);

        Task<BaseResponse<int>> DeleteBoard(int id);
    }
}
