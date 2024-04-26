using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;
using System.Xml.Linq;

namespace ControlSystem.Services.Interfaces
{
    public interface IBPMNGenerateService
    {
        BaseResponse<XDocument> GenerateProcess(string jsonBpmn);
        Task<BaseResponse<bool>> SaveBPMNToDB(string username, Chart chart);

        Task<BaseResponse<List<Chart>>> GetAllChartsByUser(string username);

        Task<BaseResponse<Chart>> GetChartById(int chartId);

        Task<BaseResponse<bool>> EditChart(int chartI, string newXmlData);

        Task<BaseResponse<bool>> DeleteChart(int chartId);

        BaseResponse<List<(string, string)>> GetTicketsFromChart(List<string> selectedTasksIds, string xmlChart);
    }
}
