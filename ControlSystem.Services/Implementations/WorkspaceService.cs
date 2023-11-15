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
    public class WorkspaceService : IWorkspaceService
    {
        private readonly ILogger<UserAccountService> _logger;

        private readonly IRepository<UserAccount> _userRpository;

        private readonly IRepository<Workspace> _workspaceRepository;

        public WorkspaceService(ILogger<UserAccountService> logger,
            IRepository<UserAccount> repository,
            IRepository<Workspace> workspaceRepository)
        {
            _logger = logger;
            _userRpository = repository;
            _workspaceRepository = workspaceRepository;
        }

        public async Task<BaseResponse<bool>> CreateWorkspace(string username, string workspaceName)
        {
            try
            {
                var user = await _userRpository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_userRpository as UserAccountRepository)!.AddWorkspaceToUser(user, new Workspace { Name = workspaceName });

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateWorkspace]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> RenameWorkspace(int id, string workspaceName)
        {
            try
            {
                var workspace = await _workspaceRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (workspace == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.WorkspaceNotFound,
                        Description = StatusCode.WorkspaceNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }
                workspace.Name = workspaceName;

                await _workspaceRepository.Update(workspace);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[RenameWorkspace]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> CreateBoard(int id, Board board)
        {
            try
            {
                var workspace = await _workspaceRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (workspace == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.WorkspaceNotFound,
                        Description = StatusCode.WorkspaceNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_workspaceRepository as WorkspaceRepository)!.AddBoard(workspace, board);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateBoard]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteWorkspace(string username, Workspace workspace)
        {
            try
            {
                var user = await _userRpository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_userRpository as UserAccountRepository)!.DeleteWorkspace(user, workspace);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteWorkspace]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }
    }
}
