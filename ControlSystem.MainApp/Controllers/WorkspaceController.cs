using ControlSystem.Domain.Entities;
using ControlSystem.Domain.ViewModels;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    [Authorize]
    public class WorkspaceController : Controller
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IBoardService _boardService;
        private readonly IUserAccountService _userService;

        public WorkspaceController(
            IWorkspaceService workspaceService,
            IBoardService boardService,
            IUserAccountService userService)
        {
            _workspaceService = workspaceService;
            _boardService = boardService;
            _userService = userService;
        }

        [HttpGet("Workspace/Workspaces/{id?}/{ticketId?}")]
        public IActionResult Workspaces(int id = 0, int ticketId = 0)
        {
            ViewBag.Workspaces = _workspaceService.GetWorkspaces(User.Identity!.Name!).Data!;

            if (id == 0)
                ViewBag.CurrentWorkspaceId = ViewBag.Workspaces[0].Id;
            else
                ViewBag.CurrentWorkspaceId = id;

            ViewBag.Boards = _workspaceService.GetBoards(ViewBag.CurrentWorkspaceId).Data;

            var tickets = new Dictionary<int, List<Ticket>>();
            foreach (var board in _workspaceService.GetBoards(id).Data)
            {
                tickets[board.Id] = _boardService.GetTickets(board.Id).Data!;
                tickets[board.Id].Reverse();
            }
            ViewBag.Tickets = tickets;

            List<Comment> comments = new();
            comments.Add(new Comment
            {
                Author = new UserAccount { Username = "Daniel" },
                Content = "features/addingServices.\r\nНачал добавление стилей для модального окна создания и редактирования тикетов."
            });
            var tags = new List<Tag>
            {
                new Tag { ColorHex = "#fc1c03", Name = "#BUG" },
                new Tag { ColorHex = "#0ffc03", Name = "#front-end" }
            };
            ViewBag.Tags = tags;
            ViewBag.Comments = comments;
            ViewBag.Author = User.Identity.Name!;
            ViewBag.UpdatedDate = DateTime.Now.ToString("dd.MM.yyyy  HH:mm");
            ViewBag.CreateDate = new DateTime(1999, 3, 4, 1, 45, 33);
            ViewBag.DeadlineDate = DateTime.Now.ToString("dd.MM.yyyy  HH:mm");
            ViewBag.Executor = "FeDDoS";
            ViewBag.Priority = new Priority { ColorHex = "#fc1c03", Name = "СРОЧНО" };

            ViewBag.TicketId = ticketId;
            ViewBag.Id = id;



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkspace(string workspaceName)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity!.Name!;
                var response = await _workspaceService.CreateWorkspace(username, workspaceName);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Workspaces = _workspaceService.GetWorkspaces(User.Identity!.Name!).Data!;
                    return View("Workspaces");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWorkspace(int id)
        {
            var username = User.Identity!.Name!;
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.DeleteWorkspace(username, id);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Workspaces = _workspaceService.GetWorkspaces(User.Identity!.Name!).Data!;
                    return View("Workspaces");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RenameWorkspace(int id, string workspaceName)
        {
            var username = User.Identity!.Name!;
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.RenameWorkspace(id, workspaceName);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Workspaces = _workspaceService.GetWorkspaces(User.Identity!.Name!).Data!;
                    return View("Workspaces");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard(int workspaceId, BoardViewModel board)
        {
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.CreateBoard(workspaceId, board);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("Workspaces", new { id = workspaceId });
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("Workspaces", new { id = workspaceId });
        }

        [HttpPost]
        public async Task<IActionResult> EditBoard(int boardId, BoardViewModel boardViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.EditBoard(boardId, boardViewModel);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("Workspaces", new { id = response.Data });
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("Workspaces");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.DeleteBoard(id);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("Workspaces", new { id = response.Data });
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("Workspaces");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketViewModel ticketVM)
        {
            var username = User.Identity!.Name!;

            if (ModelState.IsValid)
            {
                var response = await _boardService.CreateTicket(username, ticketVM);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("Workspaces", new { id = response.Data });
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("Workspaces");
        }


        [HttpGet("InviteUser/Workspace/{workspaceId}")]
        public async Task<ActionResult> InviteUserToWorkspace(int workspaceId)
        {
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.GetWorkspaceById(workspaceId);
                var userResponse = _userService.GetUser(User.Identity!.Name!);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.UserId = userResponse.Id;
                    return View(response.Data);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при прохождении по ссылке");
        }

        [HttpPost]
        public async Task<ActionResult> InviteUserToWorkspace(int workspaceId, int userId)
        {
            if (ModelState.IsValid)
            {
                var response = await _workspaceService.AddWorkspaceToUser(workspaceId, userId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("Workspaces");
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("Workspaces");
        }
    }
}
