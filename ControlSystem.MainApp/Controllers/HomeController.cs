using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Models.BPMNComponents;
using ControlSystem.MainApp.Models;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Linq;

namespace ControlSystem.MainApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUpdatesService _updatesService;

        public HomeController(ILogger<HomeController> logger,
            IUpdatesService updatesService) : base()
        {
            _logger = logger;
            _updatesService = updatesService;
        }

        [HttpGet]
        public ActionResult GenerateForms()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GenerateForms(string allData)
        {
            var bpmnElements = JsonConvert.DeserializeObject<BPMNElementsStorage>(allData);

            BPMNExtensions.FillSequenceFlows(bpmnElements.Processes[0]);
            var xml = new XDocument();
            xml.GenerateDiagramElements(bpmnElements);
            xml.SetDiagramOptions(bpmnElements);

            ViewBag.Chart = xml;

            return View("Modeler");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _updatesService.GetUpdatesInfo();

            ViewBag.AboutSite = "Иинформационная система управления задачами и бизнес-процессами";

            List<UpdateInfo> list = new();

            list.Add(new UpdateInfo
            {
                Topic = "Ну чо-то поменял ",
                Date = DateTime.Now,
                Version = "1.0-alpha",
                Description = string.Join(", ", Enumerable.Repeat("Иинформационная система управления задачами и бизнес-процессами ", 12))
            });
            list.AddRange(Enumerable.Repeat(list[0], 10));
            return View(list);
            return View(response.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}