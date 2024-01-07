using ControlSystem.Domain.Entities;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<ActionResult> AddFiles([FromForm] int ticketId,
            [FromForm] IFormFileCollection files)
        {
            if (ModelState.IsValid)
            {
                var fileList = GetFilesModels(files);

                var response = await _fileService.CreateFiles(ticketId, fileList);

                var filesResponse = await _fileService.GetFilesByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    filesResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Json(filesResponse.Data);
                }
                ModelState.AddModelError("", response.Description);
            }

            return BadRequest("Ошибка добавления файлов");
        }

        public async Task<ActionResult> DeleteFile(int fileId)
        {
            if (ModelState.IsValid)
            {
                var response = await _fileService.DeleteFile(fileId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Ok();
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка добавления файлов");
        }

        public async Task<ActionResult> DownloadFile(int fileId)
        {
            if (ModelState.IsValid)
            {
                var response = await _fileService.GetFullFileById(fileId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var fileContent = response.Data!.FileContent.Content;
                    var fileContentType = "application/octet-stream";
                    var fileName = response.Data.FileName;

                    return File(fileContent, fileContentType);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при скачивании файла");
        }

        private List<FileAttachment> GetFilesModels(IFormFileCollection files)
        {
            var fileList = new List<FileAttachment>();

            foreach (var formFile in files)
            {
                var file = new FileAttachment
                {
                    FileName = formFile.FileName,
                    FileContent = new FileContent
                    {
                        Content = GetFileBytes(formFile),
                    }
                };

                fileList.Add(file);
            }

            return fileList;
        }

        private byte[] GetFileBytes(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
