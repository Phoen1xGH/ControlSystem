using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    [Authorize]
    public class WorkspaceController : Controller
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [HttpGet]
        public IActionResult Workspaces()
        {
            ViewBag.Workspaces = _workspaceService.GetWorkspaces(User.Identity!.Name!).Data!;

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
    }
}
