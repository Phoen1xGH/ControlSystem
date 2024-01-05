using ControlSystem.Domain.Entities;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private readonly ITagService _tagsService;
        public TagsController(ITagService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTag(int ticketId, Tag tagData)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.CreateTag(tagData);

                var allTagsResponse = _tagsService.GetAllTags();

                var usedTagsResponse = _tagsService.GetTagsByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    allTagsResponse.StatusCode == Domain.Enums.StatusCode.OK &&
                    usedTagsResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var allTags = allTagsResponse.Data!;
                    var usedTags = usedTagsResponse.Data!;

                    allTags.RemoveAll(usedTags.Contains);

                    return Json(new { TicketId = ticketId, UsedTags = usedTags, UnusedTags = allTags });
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при создании тега");
        }

        [HttpPost]
        public async Task<ActionResult> EditTag(int id, Tag tag)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.EditTag(id, tag);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tags = _tagsService.GetAllTags().Data;

                    return Json(tags);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при редактировании тега");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTag(int ticketId, int tagId)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.DeleteTag(tagId);

                var allTagsResponse = _tagsService.GetAllTags();

                var usedTagsResponse = _tagsService.GetTagsByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    allTagsResponse.StatusCode == Domain.Enums.StatusCode.OK &&
                    usedTagsResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var allTags = allTagsResponse.Data!;
                    var usedTags = usedTagsResponse.Data!;

                    allTags.RemoveAll(usedTags.Contains);

                    return Json(new { TicketId = ticketId, UsedTags = usedTags, UnusedTags = allTags });
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при удалении тега");
        }

        [HttpGet]
        public ActionResult GetTags(int ticketId)
        {
            if (ModelState.IsValid)
            {
                var response = _tagsService.GetAllTags();

                var secondResponse = _tagsService.GetTagsByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    secondResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var allTags = response.Data!;

                    var usedTags = secondResponse.Data!;

                    allTags.RemoveAll(usedTags.Contains);

                    var model = (TicketId: ticketId, UsedTags: usedTags, UnusedTags: allTags);

                    return PartialView("_TagsChoice", model);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при получении тегов");
        }

        [HttpPost]
        public async Task<ActionResult> AddTagToTicket(int ticketId, int tagId)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.AddTagToTicket(ticketId, tagId);

                var allTagsResponse = _tagsService.GetAllTags();

                var usedTagsResponse = _tagsService.GetTagsByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    allTagsResponse.StatusCode == Domain.Enums.StatusCode.OK &&
                    usedTagsResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var allTags = allTagsResponse.Data!;
                    var usedTags = usedTagsResponse.Data!;

                    allTags.RemoveAll(usedTags.Contains);

                    return Json(new { TicketId = ticketId, UsedTags = usedTags, UnusedTags = allTags });
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при выборе тега");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveTagFromTicket(int ticketId, int tagId)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.RemoveTagFromTicket(ticketId, tagId);

                var allTagsResponse = _tagsService.GetAllTags();

                var usedTagsResponse = _tagsService.GetTagsByTicket(ticketId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK &&
                    allTagsResponse.StatusCode == Domain.Enums.StatusCode.OK &&
                    usedTagsResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var allTags = allTagsResponse.Data!;
                    var usedTags = usedTagsResponse.Data!;

                    allTags.RemoveAll(usedTags.Contains);

                    return Json(new { TicketId = ticketId, UsedTags = usedTags, UnusedTags = allTags });
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при выборе тега");
        }
    }
}
