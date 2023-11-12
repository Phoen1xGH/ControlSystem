using ControlSystem.Domain.Extensions;
using ControlSystem.Domain.Models.BPMNComponents;
using ControlSystem.MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Linq;

namespace ControlSystem.MainApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult Index()
        {
            return View();
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