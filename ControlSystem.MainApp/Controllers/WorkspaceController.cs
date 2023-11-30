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

        public WorkspaceController(IWorkspaceService workspaceService, IBoardService boardService)
        {
            _workspaceService = workspaceService;
            _boardService = boardService;
        }

        [HttpGet]
        public IActionResult Workspaces(int id = 1)
        {
            ViewBag.Workspaces = _workspaceService.GetWorkspaces(User.Identity!.Name!).Data!;
            ViewBag.CurrentWorkspaceId = id;
            ViewBag.Boards = _workspaceService.GetBoards(id).Data;

            var tickets = new Dictionary<int, List<Ticket>>();
            foreach (var board in _workspaceService.GetBoards(id).Data)
            {
                tickets[board.Id] = _boardService.GetTickets(board.Id).Data!;
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
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            var username = User.Identity!.Name!;

            if (ModelState.IsValid)
            {
                var response = await _boardService.CreateTicket(username, ticket);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("Workspaces", new { id = response.Data });
                }
                ModelState.AddModelError("", response.Description);
            }
            return RedirectToAction("Workspaces");
        }
    }
}
