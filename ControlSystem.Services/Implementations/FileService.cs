using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Response;
using ControlSystem.Services.DTO;
using ControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlSystem.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        private readonly IRepository<FileAttachment> _fileRepository;
        private readonly IRepository<Ticket> _ticketRepository;

        public FileService(ILogger<FileService> logger,
            IRepository<FileAttachment> fileRepository,
            IRepository<Ticket> ticketRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<BaseResponse<bool>> CreateFiles(int ticketId, List<FileAttachment> files)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                (ticket.Attachments as List<FileAttachment>)!.AddRange(files);

                await _ticketRepository.Update(ticket);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateFiles]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteFile(int fileId)
        {
            try
            {
                var file = await _fileRepository.GetAll().FirstOrDefaultAsync(x => x.Id == fileId);

                if (file is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.FileNotFound,
                        Description = StatusCode.FileNotFound.GetDescriptionValue(),
                        Data = false
                    };
                }

                await _fileRepository.Delete(file);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteFile]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<List<FileDTO>>> GetFilesByTicket(int ticketId)
        {
            try
            {
                var ticket = await _ticketRepository.GetAll().FirstOrDefaultAsync(x => x.Id == ticketId);

                if (ticket is null)
                {
                    return new BaseResponse<List<FileDTO>>
                    {
                        StatusCode = StatusCode.TicketNotFound,
                        Description = StatusCode.TicketNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                var files = ticket.Attachments.Select(x => new FileDTO { Name = x.FileName, Id = x.Id }).ToList();

                return new BaseResponse<List<FileDTO>>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = files
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetFilesByTicket]: {ex.Message}");

                return new BaseResponse<List<FileDTO>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<FileAttachment>> GetFullFileById(int fileId)
        {
            try
            {
                var file = await (_fileRepository as FileRepository)!
                    .GetAllWithContent()
                    .FirstOrDefaultAsync(x => x.Id == fileId);

                if (file is null)
                {
                    return new BaseResponse<FileAttachment>
                    {
                        StatusCode = StatusCode.FileNotFound,
                        Description = StatusCode.FileNotFound.GetDescriptionValue(),
                        Data = null
                    };
                }

                return new BaseResponse<FileAttachment>
                {
                    StatusCode = StatusCode.OK,
                    Description = StatusCode.OK.GetDescriptionValue(),
                    Data = file
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetFilesByTicket]: {ex.Message}");

                return new BaseResponse<FileAttachment>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    Data = null
                };
            }
        }
    }
}
