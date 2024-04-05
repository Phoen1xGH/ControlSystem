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

            ViewBag.InfoTitle = "Оптимизируйте процессы, подчинив себе время";

            ViewBag.AboutSite = "Time Sense Workflow - российская информационная система " +
                "для управления задачами и бизнес-процессами с \"чувством времени\". Это верный инструмент, с которым " +
                "Вы сможете оптимизировать задачи бизнеса и повседневной жизни. " +
                "В ней есть работа с привычными карточками, а также диграммами бизнес-процессов с возможностью их генерации.";

            var updates = response.Data ??= new List<UpdateInfo>();
            return View(updates);
        }

        [HttpGet("/addPatchNote")]
        public ActionResult AddPatchNote()
        {
            return View();
        }

        [HttpPost("/addPatchNote")]
        public async Task<ActionResult> AddPatchNote(string version, string topic, string description)
        {
            if (ModelState.IsValid)
            {
                var response = await _updatesService
                    .AddUpdateInfo(version, topic, description);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    return Ok();
            }
            return BadRequest();
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