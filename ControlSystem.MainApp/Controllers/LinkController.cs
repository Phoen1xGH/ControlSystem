using ControlSystem.Domain.Entities;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILinkService _linkService;

        public LinkController(ILinkService priorityService)
        {
            _linkService = priorityService;
        }

        public async Task<ActionResult> CreateLink(int ticketId, Link link)
        {
            if (ModelState.IsValid)
            {
                var response = await _linkService.CreateLink(ticketId, link);

                var linksResponse = await _linkService.GetLinksByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    linksResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var links = linksResponse.Data!;

                    return Json(links);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при добавлении ссылки");
        }

        public async Task<ActionResult> DeleteLink(int linkId)
        {
            if (ModelState.IsValid)
            {
                var response = await _linkService.DeleteLink(linkId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Ok();
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при удалении ссылки");
        }

        public async Task<ActionResult> EditLink(Link newLinkData)
        {
            if (ModelState.IsValid)
            {
                var response = await _linkService.EditLink(newLinkData.Id, newLinkData);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Json(newLinkData);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при редактировании ссылки");
        }
    }
}
