using ControlSystem.MainApp.ViewModels;
using ControlSystem.Services.DTO;
using ControlSystem.Services.Implementations;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    /// <summary>
    /// Контроллер для управления карточками
    /// </summary>
    public class TicketController : Controller
    {
        #region fields

        /// <summary>
        /// Сервис досок
        /// </summary>
        private readonly IBoardService _boardService;

        /// <summary>
        /// Сервис рабочих пространств
        /// </summary>
        private readonly IWorkspaceService _workspaceService;

        #endregion

        #region constructors

        public TicketController(IBoardService boardService,
            IWorkspaceService workspaceService)
        {
            _boardService = boardService;
            _workspaceService = workspaceService;
        }

        #endregion

        #region actions

        /// <summary>
        /// Создать карточку
        /// </summary>
        /// <param name="title">заголовок</param>
        /// <param name="boardId">идентификатор доски</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удалить карточку
        /// </summary>
        /// <param name="ticketId">идентификатор карточки</param>
        /// <param name="boardId">идентификатор доски</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteTicket(int ticketId, int boardId)
        {
            if (ModelState.IsValid)
            {
                var response = await _boardService.DeleteTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Ok();
                }
            }
            return BadRequest("Ошибка при удалении тикета");
        }

        /// <summary>
        /// Редактировать карточку
        /// </summary>
        /// <param name="newTicketData">новые данные</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditTicket(TicketDTO newTicketData)
        {
            var username = User.Identity!.Name!;

            if (ModelState.IsValid)
            {
                var ticketData = new TicketChangesDTO
                {
                    Id = newTicketData.Id,
                    Title = newTicketData.Title,
                    Description = newTicketData.Description
                };

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

        /// <summary>
        /// Получить содержимое карточки
        /// </summary>
        /// <param name="workspaceId">идентификатор пространства</param>
        /// <param name="ticketId">идентификатор карточки</param>
        /// <returns></returns>
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

                    var statuses = _workspaceService.GetBoards(workspaceId).Data!
                        .Select(x => new BoardDTO
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToList();

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
                        Statuses = statuses
                    };

                    return PartialView("_TicketDetails", model);
                }
            }
            return BadRequest("Произошла ошибка при открытии задачи");
        }

        /// <summary>
        /// Добавить исполнителя
        /// </summary>
        /// <param name="ticketId">идентификатор карточки</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удалить пользователя из участников карточки
        /// </summary>
        /// <param name="ticketId">идентификатор карточки</param>
        /// <param name="participantId">идентификатор участника</param>
        /// <returns></returns>
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

        /// <summary>
        /// Сменить статус карточки
        /// </summary>
        /// <param name="ticketId">идентификатор карточки</param>
        /// <param name="boardId">идентификатор нового статуса</param>
        /// <returns></returns>
        public async Task<ActionResult> ChangeStatus(int ticketId, int boardId)
        {
            if (ModelState.IsValid)
            {
                var response = await _boardService.ChangeStatus(ticketId, boardId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var ticketsResponse = _boardService.GetTickets(boardId);

                    var tickets = ticketsResponse.Data.Select(ticket => new TicketPreviewViewModel
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        StatusId = ticket.Status.Id,
                        WorkspaceId = ticket.Status.Workspace.Id,
                        Priority = ticket.Priority
                    });

                    var workspaceIdResponse = await _boardService.GetWorkspaceId(boardId);

                    var workspaceId = workspaceIdResponse.Data;

                    var statuses = _workspaceService.GetBoards(workspaceId);

                    var newStatuses = statuses.Data!.Where(status => status.Id != boardId).Select(status => new BoardDTO
                    {
                        Id = status.Id,
                        Name = status.Name
                    }).OrderBy(status => status.Id);

                    var currentStatus = statuses.Data!.FirstOrDefault(status => status.Id == boardId)!.Name;

                    return Json(new { Tickets = tickets.Reverse(), Statuses = newStatuses, CurrentStatusName = currentStatus });
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при смене статуса");
        }

        #endregion

        public void AddPartsToAllTicket()
        {
            (_boardService as BoardService)!.AddParts();
        }
    }
}
