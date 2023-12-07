using ControlSystem.Domain.Entities;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    public class PriorityController : Controller
    {
        private readonly IPriorityService _priorityService;

        public PriorityController(IPriorityService priorityService)
        {
            _priorityService = priorityService;
        }

        public ActionResult Priorities()
        {
            var priorities = _priorityService.GetPriorities();

            return Json(priorities);
        }

        public async Task<ActionResult> CreatePriority(Priority priority)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.CreatePriority(priority);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Priorities");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Priorities");
        }

        public async Task<ActionResult> EditPriority(int id, Priority priority)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.EditPriority(id, priority);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Priorities");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Priorities");
        }

        public async Task<ActionResult> DeletePriority(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.DeletePriority(id);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Priorities");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Priorities");
        }
    }
}
