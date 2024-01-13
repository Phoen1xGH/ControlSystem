using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<ActionResult> AddComment(int ticketId, string content)
        {
            if (ModelState.IsValid)
            {
                var response = await _commentService
                    .CreateComment(ticketId, User.Identity!.Name!, content);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Json(response.Data);
                }
                ModelState.AddModelError("", response.Description);
            }
            return BadRequest("Ошибка при отправке комментария");
        }
    }
}
