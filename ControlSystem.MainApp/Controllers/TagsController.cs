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


        public ActionResult Tags()
        {
            var tags = _tagsService.GetAllTags().Data;

            return Json(tags);
        }

        public async Task<ActionResult> CreateTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.CreateTag(tag);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Tags");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Tags");
        }

        public async Task<ActionResult> EditTag(int id, Tag tag)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.EditTag(id, tag);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Tags");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Tags");
        }

        public async Task<ActionResult> DeleteTag(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagsService.DeleteTag(id);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Tags");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Tags");
        }
    }
}
