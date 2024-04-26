using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Models.BPMNComponents;
using ControlSystem.Domain.Models.BPMNComponents.Elements;
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
        private readonly ILogger<BPMNGenerateService> _logger;

        private readonly IRepository<UserAccount> _userRepository;

        private readonly IRepository<Chart> _chartRepository;

        public BPMNGenerateService(ILogger<BPMNGenerateService> logger,
            IRepository<UserAccount> userRepo,
            IRepository<Chart> chartRepo)
        {
            _logger = logger;
            _userRepository = userRepo;
            _chartRepository = chartRepo;
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

                await (_userRepository as UserAccountRepository)!.AddChartToUser(user, chart);

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

        public async Task<BaseResponse<List<Chart>>> GetAllChartsByUser(string username)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == username);

                if (user is null)
                {
                    return new BaseResponse<List<Chart>>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = StatusCode.UserNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                List<Chart> charts = user.Charts.ToList();

                return new BaseResponse<List<Chart>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = charts
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAllChartsByUser]: {ex.Message}");

                return new BaseResponse<List<Chart>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Chart>> GetChartById(int chartId)
        {
            try
            {
                var chart = await _chartRepository.GetAll().FirstOrDefaultAsync(x => x.Id == chartId);

                if (chart == null)
                {
                    return new BaseResponse<Chart>
                    {
                        StatusCode = StatusCode.ChartNotFound,
                        Description = StatusCode.ChartNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                return new BaseResponse<Chart>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = chart
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetChartById]: {ex.Message}");

                return new BaseResponse<Chart>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> EditChart(int chartId, string newXmlData)
        {
            try
            {
                var chart = await _chartRepository.GetAll().FirstOrDefaultAsync(x => x.Id == chartId);

                if (chart == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.ChartNotFound,
                        Description = StatusCode.ChartNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                chart.XmlData = newXmlData;

                await _chartRepository.Update(chart);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EditChart]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteChart(int chartId)
        {
            try
            {
                var chart = await _chartRepository.GetAll().FirstOrDefaultAsync(x => x.Id == chartId);

                if (chart == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.ChartNotFound,
                        Description = StatusCode.ChartNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _chartRepository.Delete(chart);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteChart]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public BaseResponse<List<(string, string)>> GetTicketsFromChart(List<string> selectedTasksIds, string xmlChart)
        {
            try
            {
                List<(string, string)> taskNames = new List<(string, string)>();

                XDocument doc = XDocument.Parse(xmlChart);

                var taskElements = doc.Descendants().Where(e => e.Name.LocalName == "task");

                if (selectedTasksIds.Count > 0)
                    taskElements = taskElements.Where(el => selectedTasksIds.Contains(el.Attribute("id")?.Value!));

                // Создаем список для хранения информации о задачах
                var tasksInfo = new List<BPMNTask>();

                // Добавляем информацию о задачах в список
                foreach (var taskElement in taskElements)
                {
                    var taskId = taskElement.Attribute("id")?.Value;
                    var taskName = taskElement.Attribute("name")?.Value;

                    // Находим входящую и исходящую связь для текущей задачи
                    var incoming = taskElement.Elements().FirstOrDefault(e => e.Name.LocalName == "incoming")?.Value;
                    var outgoing = taskElement.Elements().FirstOrDefault(e => e.Name.LocalName == "outgoing")?.Value;

                    tasksInfo.Add(new BPMNTask
                    {
                        Name = taskName,
                        Id = taskId,
                        Incoming = incoming,
                        Outgoing = outgoing
                    });
                }

                for (int i = 0; i < tasksInfo.Count; i++)
                {
                    var currentTask = tasksInfo[i];

                    for (int j = 0; j < tasksInfo.Count; j++)
                    {
                        var otherTask = tasksInfo[j];

                        if (currentTask.Outgoing == otherTask.Incoming)
                            taskNames.Add((currentTask.Name, otherTask.Name));
                    }
                }

                return new BaseResponse<List<(string, string)>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = taskNames
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetTicketsFromChart]: {ex.Message}");

                return new BaseResponse<List<(string, string)>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
