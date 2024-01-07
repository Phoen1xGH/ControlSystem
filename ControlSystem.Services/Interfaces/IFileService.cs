using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Response;
using ControlSystem.Services.DTO;

namespace ControlSystem.Services.Interfaces
{
    public interface IFileService
    {
        Task<BaseResponse<bool>> CreateFiles(int ticketId, List<FileAttachment> files);

        Task<BaseResponse<bool>> DeleteFile(int fileId);

        Task<BaseResponse<List<FileDTO>>> GetFilesByTicket(int ticketId);

        Task<BaseResponse<FileAttachment>> GetFullFileById(int fileId);
    }
}
