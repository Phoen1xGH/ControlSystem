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

            controller!.ViewBag.Title = GetPageTitle(controller);
        }

        public void OnActionExecuting(ActionExecutingContext _) { }

        private static string GetPageTitle(Controller controller) => controller switch
        {
            AccountController => "Аккаунт",
            BPMNChartsController => "Бизнес-процессы",
            HomeController => "Информация",
            WorkspaceController => "Задачи",
            _ => "ИСУЗП",
        };
    }
}
