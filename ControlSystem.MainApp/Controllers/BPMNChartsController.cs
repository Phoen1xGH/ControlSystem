using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlSystem.MainApp.Controllers
{
    [Authorize]
    public class BPMNChartsController : Controller
    {
        private readonly IBPMNGenerateService _accountService;

        public BPMNChartsController(IBPMNGenerateService service)
        {
            _accountService = service;
        }

        [HttpGet]
        public IActionResult GenerateForms()
            => View();

        [HttpPost]
        public IActionResult GenerateForms(string allData)
        {
            if (ModelState.IsValid)
            {
                var response = _accountService.GenerateProcess(allData);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Chart = response.Data;

                    return View("Modeler");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View();
        }
    }
}
