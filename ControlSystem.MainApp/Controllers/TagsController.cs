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
        public async Task<ActionResult> CreateTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.CreateTag(tag);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tags = _tagsService.GetAllTags().Data;

                    return Json(tags);
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
        public async Task<ActionResult> DeleteTag(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.DeleteTag(id);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    var tags = _tagsService.GetAllTags().Data;

                    return Json(tags);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при удалении тега");
        }
    }
}
