using ControlSystem.Domain.Entities;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ControlSystem.MainApp.Controllers
{
    [Authorize]
    public class BPMNChartsController : BaseController
    {
        private readonly IBPMNGenerateService _chartService;
        private readonly IBoardService _boardService;
        private readonly IUserAccountService _userService;
        private readonly ILinkService _linkService;

        public BPMNChartsController(IBPMNGenerateService service,
            IBoardService boardService,
            IUserAccountService userService,
            ILinkService linkService) : base()
        {
            _chartService = service;
            _boardService = boardService;
            _userService = userService;
            _linkService = linkService;
        }

        [HttpGet]
        public IActionResult GenerateForms()
            => View();

        [HttpPost]
        public IActionResult GenerateForms(string allData)
        {
            if (ModelState.IsValid)
            {
                var response = _chartService.GenerateProcess(allData);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Chart = response.Data;

                    return View("Modeler");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChartToDb(string jsonChart)
        {
            var chart = JsonConvert.DeserializeObject<Chart>(jsonChart);
            if (ModelState.IsValid)
            {
                var currentUserName = User.Identity!.Name!;
                var response = await _chartService.SaveBPMNToDB(currentUserName, chart);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Chart = chart.XmlData;

                    var charts = await _chartService.GetAllChartsByUser(currentUserName);
                    ViewBag.Id = charts.Data!.OrderBy(ch => ch.Id).Last().Id;

                    return View("Modeler");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Modeler()
        {
            ViewBag.Chart = """
                <?xml version="1.0" encoding="UTF-8"?>
                <bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" id="Definitions_1c023tk" targetNamespace="http://bpmn.io/schema/bpmn" exporter="bpmn-js (https://demo.bpmn.io)" exporterVersion="16.3.2">
                  <bpmn:process id="Process_18trbpf" isExecutable="false">
                    <bpmn:startEvent id="StartEvent_01oljht" />
                  </bpmn:process>
                  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
                    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_18trbpf">
                      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_01oljht">
                        <dc:Bounds x="156" y="82" width="36" height="36" />
                      </bpmndi:BPMNShape>
                    </bpmndi:BPMNPlane>
                  </bpmndi:BPMNDiagram>
                </bpmn:definitions>
                
                """;

            SetupWorkspacesList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ModelerByChart(int chartId)
        {
            if (ModelState.IsValid)
            {
                var response = await _chartService.GetChartById(chartId);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Chart = response.Data!.XmlData;
                    ViewBag.Id = response.Data!.Id;
                    SetupWorkspacesList();
                    return View("Modeler");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllCharts()
        {
            var username = User.Identity!.Name!;

            var response = await _chartService.GetAllChartsByUser(username);

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                response.Data!.Reverse();
                return View(response.Data!);
            }
            ModelState.AddModelError("", response.Description);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditChart(int chartId, string newXmlData)
        {
            if (ModelState.IsValid)
            {
                var response = await _chartService.EditChart(chartId, newXmlData);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Id = chartId;
                    return Ok();
                }
            }
            return BadRequest("Произошла ошибка при редактировании диаграммы");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChart(int chartId)
        {
            if (ModelState.IsValid)
            {
                var delResponse = await _chartService.DeleteChart(chartId);

                if (delResponse.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return Ok();
                }
            }
            return BadRequest("Ошибка при удалении диаграммы");
        }


        [HttpPost]
        public IActionResult ImportBPMNFile([FromForm] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                using var reader = new StreamReader(file.OpenReadStream());

                string xmlChart = reader.ReadToEnd();

                return Json(xmlChart);
            }
            return BadRequest("Ошибка при импорте файла");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicketsFromChartTasks(int workspaceId, int boardId, List<string> selectedTasksIds, string xmlChart)
        {
            //if (ModelState.IsValid)
            //{
            //    bool isSuccess;

            //    var getTitlesResponse = _chartService.GetTicketsFromChart(xmlChart);

            //    if (getTitlesResponse.StatusCode == Domain.Enums.StatusCode.OK)
            //    {
            //        isSuccess = await CreateTicketsResponseAsync(getTitlesResponse.Data!, boardId);

            //        if (isSuccess)
            //            isSuccess = await AddLinksToNewTicketsAsync(workspaceId, boardId, getTitlesResponse.Data.Count);

            //        if(isSuccess)
            //            return Ok();

            //    }
            //}
            //return BadRequest("Произошла ошибка при импорте диаграммы в карточки");

            if (!ModelState.IsValid)
                return BadRequest("Произошла ошибка");

            var getTitlesResponse = _chartService.GetTicketsFromChart(selectedTasksIds, xmlChart);
            if (getTitlesResponse.StatusCode != Domain.Enums.StatusCode.OK)
                return BadRequest("Произошла ошибка при обработке диаграммы");

            var tickets = new HashSet<string>();

            getTitlesResponse.Data!.ForEach(tuple =>
            {
                tickets.Add(tuple.Item1);
                tickets.Add(tuple.Item2);
            });

            if (!await CreateTicketsResponseAsync(tickets, boardId) ||
                !await AddLinksToNewTicketsAsync(workspaceId, boardId, tickets.Count, getTitlesResponse.Data!))
                return BadRequest("Произошла ошибка при создании карточек или добавлении ссылок");

            return Ok();
        }


        private async Task<bool> CreateTicketsResponseAsync(IEnumerable<string> names, int boardId)
        {
            foreach (var name in names)
            {
                var createResponse = await _boardService.CreateTicket(User.Identity!.Name!, name, boardId);

                if (createResponse.StatusCode != Domain.Enums.StatusCode.OK)
                    return false;
            }
            return true;
        }

        private async Task<bool> AddLinksToNewTicketsAsync(int workspaceId, int boardId, int ticketsCount, List<(string, string)> ticketsTuple)
        {
            var ticketsResponse = _boardService.GetTickets(boardId);

            if (ticketsResponse.StatusCode != Domain.Enums.StatusCode.OK)
                return false;

            var tickets = ticketsResponse.Data!.TakeLast(ticketsCount).ToList();

            foreach (var currentTicket in tickets)
            {
                var linkedTicketTuple = ticketsTuple.FirstOrDefault(t => t.Item1 == currentTicket.Title);
                if (linkedTicketTuple.Item2 is null)
                    continue;
                var linkedTicketName = linkedTicketTuple.Item2;

                var linkedTicket = tickets.FirstOrDefault(t => t.Title == linkedTicketName);

                if (linkedTicket is null)
                    continue;

                var link = new Link
                {
                    Name = $"Следующий этап (Карточка {linkedTicket!.Id})",
                    Source = $"{HttpContext.Request.Scheme}://{Request.Host}/Workspace/Workspaces/{workspaceId}/{linkedTicket.Id}"
                };

                var linkResponse = await _linkService.CreateLink(currentTicket!.Id, link);

                if (linkResponse.StatusCode != Domain.Enums.StatusCode.OK)
                    return false;
            }

            return true;
        }


        private void SetupWorkspacesList()
        {
            UserAccount user = _userService.GetUser(User.Identity!.Name!);
            var workspaces = user.Workspaces
                                .OrderBy(w => w.Name)
                                .Select(w => new { w.Id, w.Name })
                                .ToList();

            var boards = user.Workspaces
                            .SelectMany(w => w.Boards)
                            .OrderBy(b => b.Name)
                            .Select(b => new { b.Id, b.Name, WorkspaceId = b.Workspace.Id })
                            .ToList();

            string workspacesJson = JsonConvert.SerializeObject(workspaces);
            string boardsJson = JsonConvert.SerializeObject(boards);

            ViewBag.Workspaces = workspacesJson;
            ViewBag.Boards = boardsJson;
        }
    }
}
