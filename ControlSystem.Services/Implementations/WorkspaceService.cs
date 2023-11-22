using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Domain.ViewModels;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly ILogger<UserAccountService> _logger;

        private readonly IRepository<UserAccount> _userRepository;

        private readonly IRepository<Workspace> _workspaceRepository;

        private readonly IRepository<Board> _boardRepository;

        public WorkspaceService(ILogger<UserAccountService> logger,
            IRepository<UserAccount> repository,
            IRepository<Workspace> workspaceRepository,
            IRepository<Board> boardRepository)
        {
            _logger = logger;
            _userRepository = repository;
            _workspaceRepository = workspaceRepository;
            _boardRepository = boardRepository;
        }

        public async Task<BaseResponse<bool>> CreateWorkspace(string username, string workspaceName)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_userRepository as UserAccountRepository)!.AddWorkspaceToUser(user, new Workspace { Name = workspaceName });

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

        public async Task<BaseResponse<bool>> CreateBoard(int id, BoardViewModel boardVM)
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

                Board board = new Board { Name = boardVM.Name, ColorHex = boardVM.ColorHex };

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

        public async Task<BaseResponse<int>> EditBoard(int id, BoardViewModel boardViewModel)
        {
            try
            {
                var board = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (board == null)
                {
                    return new BaseResponse<int>
                    {
                        StatusCode = StatusCode.BoardNotFound,
                        Description = StatusCode.BoardNotFound.GetDescriptionValue(),
                        Data = -1
                    };
                }

                board.Name = boardViewModel.Name;
                board.ColorHex = boardViewModel.ColorHex;

                await _boardRepository.Update(board);

                return new BaseResponse<int>
                {
                    StatusCode = StatusCode.OK,
                    Data = board.Workspace.Id,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditBoard]: {ex.Message}");

                return new BaseResponse<int>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = -1
                };
            }
        }
        public async Task<BaseResponse<bool>> DeleteWorkspace(string username, int workspaceId)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == username);
                var workspace = await _workspaceRepository.GetAll().FirstOrDefaultAsync(x => x.Id == workspaceId);
                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }
                if (workspace == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.WorkspaceNotFound,
                        Description = StatusCode.WorkspaceNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_userRepository as UserAccountRepository)!.DeleteWorkspace(user, workspace);

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

        public async Task<BaseResponse<int>> DeleteBoard(int id)
        {
            try
            {
                var boardToDelete = await _boardRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (boardToDelete is null)
                {
                    return new BaseResponse<int>
                    {
                        StatusCode = StatusCode.BoardNotFound,
                        Description = StatusCode.BoardNotFound.GetDescriptionValue(),
                        Data = -1
                    };
                }

                await _boardRepository.Delete(boardToDelete);

                return new BaseResponse<int>
                {
                    StatusCode = StatusCode.OK,
                    Data = boardToDelete.Workspace.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteBoard]: {ex.Message}");

                return new BaseResponse<int>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = -1
                };
            }
        }

        public BaseResponse<List<Workspace>> GetWorkspaces(string username)
        {
            try
            {
                var workspaces = _userRepository.GetAll().FirstOrDefault(x => x.Username == username)!.Workspaces.ToList();

                return new BaseResponse<List<Workspace>>()
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = workspaces
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetWorkspaces]: {ex.Message}");

                return new BaseResponse<List<Workspace>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                };
            }
        }

        public BaseResponse<List<Board>> GetBoards(int workspaceId)
        {
            try
            {
                var boards = _workspaceRepository.GetAll().FirstOrDefault(x => x.Id == workspaceId)!.Boards.ToList();

                return new BaseResponse<List<Board>>()
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = boards
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetBoards]: {ex.Message}");

                return new BaseResponse<List<Board>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                };
            }
        }
    }
}
