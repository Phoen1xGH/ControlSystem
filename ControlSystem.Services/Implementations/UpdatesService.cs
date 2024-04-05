using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class UpdatesService : IUpdatesService
    {
        private readonly ILogger<UpdatesService> _logger;

        private readonly IRepository<UpdateInfo> _updatesRepository;

        public UpdatesService(ILogger<UpdatesService> logger, IRepository<UpdateInfo> updatesRepository)
        {
            _logger = logger;
            _updatesRepository = updatesRepository;
        }

        public async Task<BaseResponse<bool>> AddUpdateInfo(string version, string topic, string description)
        {
            try
            {
                var newUpdate = new UpdateInfo
                {
                    Topic = topic,
                    Description = description,
                    Version = version,
                    Date = DateTime.Now
                };

                await _updatesRepository.Create(newUpdate);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AddUpdateInfo]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteUpdateInfo(int updateId)
        {
            try
            {
                var update = _updatesRepository.GetAll().FirstOrDefault(x => x.Id == updateId);

                if (update == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UpdateNotFound,
                        Description = StatusCode.UpdateNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _updatesRepository.Delete(update);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteUpdateInfo]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<UpdateInfo>> GetUpdatesInfo()
        {
            try
            {
                var allUpdates = _updatesRepository.GetAll().ToList();

                return new BaseResponse<List<UpdateInfo>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = allUpdates
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetUpdatesInfo]: {ex.Message}");

                return new BaseResponse<List<UpdateInfo>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
