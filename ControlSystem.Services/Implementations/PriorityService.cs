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
    public class PriorityService : IPriorityService
    {
        private readonly ILogger<PriorityService> _logger;

        private readonly IRepository<Priority> _priorityRepository;

        public PriorityService(ILogger<PriorityService> logger, IRepository<Priority> priorityRepo)
        {
            _logger = logger;
            _priorityRepository = priorityRepo;
        }

        public async Task<BaseResponse<bool>> CreatePriority(Priority priority)
        {
            try
            {
                await _priorityRepository.Create(priority);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreatePriority]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeletePriority(int priorityId)
        {
            try
            {
                var priority = await _priorityRepository.GetAll().FirstOrDefaultAsync(x => x.Id == priorityId);

                if (priority is null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.PriorityNotFound,
                        Description = StatusCode.PriorityNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _priorityRepository.Delete(priority);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeletePriority]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<Priority>> GetPriorities()
        {
            try
            {
                var priorities = _priorityRepository.GetAll().ToList();

                return new BaseResponse<List<Priority>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = priorities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetPriorities]: {ex.Message}");

                return new BaseResponse<List<Priority>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> EditPriority(int priorityId, Priority newDataPriority)
        {
            try
            {
                var priority = await _priorityRepository.GetAll().FirstOrDefaultAsync();

                if (priority is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.PriorityNotFound,
                        Description = StatusCode.PriorityNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _priorityRepository.Update(priority);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditPriority]: {ex.Message}");

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
