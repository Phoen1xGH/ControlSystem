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

        public async Task<ActionResult> CreatePriority(Priority priority)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.CreatePriority(priority);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var priorities = _priorityService.GetPriorities().Data;

                    return Json(priorities);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при создании приоритетности");
        }

        public async Task<ActionResult> EditPriority(int id, Priority priority)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.EditPriority(id, priority);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var priorities = _priorityService.GetPriorities().Data;

                    return Json(priorities);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при редактировании приоритетности");
        }

        public async Task<ActionResult> DeletePriority(int priorityId)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.DeletePriority(priorityId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var priorities = _priorityService.GetPriorities().Data;

                    return Json(priorities);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при удалении приоритетности");
        }

        public async Task<ActionResult> AddPriorityToTicket(int ticketId, int priorityId)
        {
            if (ModelState.IsValid)
            {
                var response = await _priorityService.AddPriorityToTicket(ticketId, priorityId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var priotity = response.Data;/*_priorityService.GetPriorityByTicket(ticketId);*/

                    return Json(priotity);
                }
            }
            return BadRequest("Ошибка при добавлении приоритета для задачи");
        }

        public ActionResult GetPriorities(int ticketId)
        {
            var response = _priorityService.GetPriorities();

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                var priorities = new Tuple<int, List<Priority>>(ticketId, response.Data!);

                return PartialView("_PriorityChoice", priorities);
            }
            return BadRequest("Ошибка при открытии окна приоритетов");
        }
    }
}
