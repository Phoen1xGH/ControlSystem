using ControlSystem.MainApp.DTO;
using ControlSystem.Services.DTO;
using ControlSystem.Services.Implementations;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    public class TicketController : Controller
    {
        private readonly IBoardService _boardService;

        public TicketController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(string title, int boardId)
        {
            var username = User.Identity!.Name!;

            if (ModelState.IsValid)
            {
                var response = await _boardService.CreateTicket(username, title, boardId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tickets = _boardService.GetTickets(boardId);
                    tickets.Data.Reverse();
                    var ticketsToJson = tickets.Data.Select(ticket => new TicketPreviewViewModel
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id,
                        Priority = ticket.Priority
                    }).ToList();

                    return Json(ticketsToJson);
                }
            }

            return BadRequest("Ошибка при создании тикета");
        }


        [HttpPost]
        public async Task<ActionResult> DeleteTicket(int ticketId, int boardId)
        {
            if (ModelState.IsValid)
            {
                var response = await _boardService.DeleteTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tickets = _boardService.GetTickets(boardId);

                    var ticketsToJson = tickets.Data.Select(ticket => new TicketPreviewViewModel
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id,
                        Priority = ticket.Priority
                    }).ToList();

                    ticketsToJson.Reverse();

                    return Json(ticketsToJson);
                }
            }
            return BadRequest("Ошибка при удалении тикета");
        }

        [HttpPost]
        public async Task<ActionResult> EditTicket(TicketDTO newTicketData)
        {
            var username = User.Identity!.Name!;

            if (ModelState.IsValid)
            {
                var ticketData = new TicketChangesDTO { Id = newTicketData.Id, Title = newTicketData.Title, Description = newTicketData.Description };
                var response = await _boardService.EditTicket(newTicketData.Id, ticketData);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tickets = _boardService.GetTickets(newTicketData.StatusId);

                    var ticketsToJson = tickets.Data.Select(ticket => new TicketPreviewViewModel
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id,
                        Priority = ticket.Priority
                    }).ToList();

                    ticketsToJson.Reverse();

                    return Json(ticketsToJson);
                }
            }
            return BadRequest("Ошибка при редактировании тикета");
        }

        [HttpGet]
        public async Task<ActionResult> TicketDetails(int workspaceId, int ticketId)
        {
            if (ModelState.IsValid)
            {
                var response = await _boardService.GetTicketById(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var ticket = response.Data;

                    var author = new UserDTO { Id = ticket.Author.Id, Name = ticket.Author.Username };

                    var executor = ticket.Executor is not null ?
                        new UserDTO { Id = ticket.Executor.Id, Name = ticket.Executor.Username } :
                        null;

                    var participants = ticket.Participants.Select(x => new UserDTO { Id = x.Id, Name = x.Username }).ToList();

                    var files = ticket.Attachments.Select(x => new FileDTO { Id = x.Id, Name = x.FileName }).ToList();

                    var comments = ticket.Comments.Select(x => new CommentDTO
                    {
                        Id = x.Id,
                        AuthorName = x.Author.Username,
                        Content = x.Content,
                        CreationDate = x.CreationDate.ToString("dd.MM.yyyy  HH:mm")
                    }).ToList();

                    comments.Reverse();

                    var model = new TicketDTO
                    {
                        Id = ticket.Id,
                        Author = author,
                        Executor = executor,
                        Participants = participants,
                        Description = ticket.Description,
                        Title = ticket.Title!,
                        UpdatedDate = ticket.UpdatedDate,
                        CreationDate = ticket.CreationDate,
                        Files = files,
                        Links = ticket.Links.ToList(),
                        Comments = comments,
                        Priority = ticket.Priority,
                        StatusId = ticket.Status.Id,
                        Tags = ticket.Tags.ToList(),
                    };

                    return PartialView("_TicketDetails", model);
                }
            }
            return BadRequest("Произошла ошибка при открытии задачи");
        }

        public async Task<ActionResult> AddExecutor(int ticketId, int userId)
        {
            if (ModelState.IsValid)
            {
                var response = await _boardService.AddExecutorToTicket(ticketId, userId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Json(response.Data);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при добавлении ответственного");
        }

        public async Task<ActionResult> RemoveParticipant(int ticketId, int participantId)
        {
            if (ModelState.IsValid)
            {
                var response = await _boardService.RemoveParticipant(ticketId, participantId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Ok();
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при удалении участника");
        }

        public void AddPartsToAllTicket()
        {
            (_boardService as BoardService)!.AddParts();
        }
    }
}
