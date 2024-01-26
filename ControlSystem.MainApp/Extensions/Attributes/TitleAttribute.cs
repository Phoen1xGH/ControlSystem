using ControlSystem.MainApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControlSystem.MainApp.Extensions.Attributes
{
    public class TitleAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;

            controller!.ViewBag.Title = GetController(controller);
        }

        public void OnActionExecuting(ActionExecutingContext _) { }

        private static string GetController(Controller controller) => controller switch
        {
            AccountController => "Аккаунт",
            BPMNChartsController => "Бизнес-процессы",
            HomeController => "Информация",
            WorkspaceController => "Задачи",
            _ => "ИСУЗП",
        };
    }
}
