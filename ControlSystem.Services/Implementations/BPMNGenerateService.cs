using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Models.BPMNComponents;
using ControlSystem.Domain.Response;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace ControlSystem.Services.Implementations
{
    public class BPMNGenerateService : IBPMNGenerateService
    {
        private readonly ILogger<UserAccountService> _logger;

        private readonly IRepository<UserAccount> _repository;

        public BPMNGenerateService(ILogger<UserAccountService> logger, IRepository<UserAccount> repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public BaseResponse<XDocument> GenerateProcess(string jsonBpmn)
        {
            try
            {
                var bpmnElements = JsonConvert.DeserializeObject<BPMNElementsStorage>(jsonBpmn);

                BPMNExtensions.FillSequenceFlows(bpmnElements.Processes[0]);
                var xml = new XDocument();
                xml.GenerateDiagramElements(bpmnElements);
                xml.SetDiagramOptions(bpmnElements);

                return new BaseResponse<XDocument>
                {
                    StatusCode = StatusCode.OK,
                    Data = xml,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GenerateProcess]: {ex.Message}");

                return new BaseResponse<XDocument>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                };
            }
        }

        public async Task<BaseResponse<bool>> SaveBPMNToDB(string username, Chart chart)
        {
            try
            {
                var user = await _repository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await (_repository as UserAccountRepository)!.AddChartToUser(user, chart);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[SaveBPMNToDB]: {ex.Message}");

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
