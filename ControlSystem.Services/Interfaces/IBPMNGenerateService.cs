using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;
using System.Xml.Linq;

namespace ControlSystem.Services.Interfaces
{
    public interface IBPMNGenerateService
    {
        BaseResponse<XDocument> GenerateProcess(string jsonBpmn);
        Task<BaseResponse<bool>> SaveBPMNToDB(string username, Chart chart);
    }
}
