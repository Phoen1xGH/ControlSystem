using ControlSystem.Domain.ViewModels;
using ControlSystem.MainApp.DTO;
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
                    var ticketsToJson = tickets.Data.Select(ticket => new TicketPreviewDTO
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id
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

                    var ticketsToJson = tickets.Data.Select(ticket => new TicketPreviewDTO
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id
                    }).ToList();

                    return Json(ticketsToJson);
                }
            }
            return BadRequest("Ошибка при удалении тикета");
        }

        [HttpPost]
        public async Task<ActionResult> EditTicket(int id, TicketViewModel newTicketData)
        {
            var username = User.Identity!.Name!;

            if (ModelState.IsValid)
            {
                var response = await _boardService.EditTicket(id, newTicketData);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tickets = _boardService.GetTickets(newTicketData.StatusId);

                    var ticketsToJson = tickets.Data.Select(ticket => new TicketPreviewDTO
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id
                    }).ToList();

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

                    var model = new TicketViewModel
                    {
                        AuthorName = ticket.Author.Username,
                        Description = ticket.Description,
                        Title = ticket.Title,
                        UpdatedDate = ticket.UpdatedDate,
                        CreationDate = ticket.CreationDate,
                    };

                    return PartialView("_TicketDetails", model);
                }
            }
            return BadRequest("Произошла ошибка при открытии задачи");
        }
    }
}
