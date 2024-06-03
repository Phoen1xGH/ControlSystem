using ControlSystem.Domain.Entities;
using ControlSystem.Services.Implementations;
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
        private readonly IFileService _fileService;

        public BPMNChartsController(IBPMNGenerateService service,
            IBoardService boardService,
            IUserAccountService userService,
            ILinkService linkService,
            IFileService fileService) : base()
        {
            _chartService = service;
            _boardService = boardService;
            _userService = userService;
            _linkService = linkService;
            _fileService = fileService;
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
            ViewBag.Chart = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" id=\"Definitions_1c023tk\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"16.3.2\">\r\n  <bpmn:process id=\"Process_18trbpf\" isExecutable=\"false\">\r\n    <bpmn:startEvent id=\"StartEvent_01oljht\" />\r\n  </bpmn:process>\r\n  <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\">\r\n    <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"Process_18trbpf\">\r\n      <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_01oljht\">\r\n        <dc:Bounds x=\"156\" y=\"82\" width=\"36\" height=\"36\" />\r\n      </bpmndi:BPMNShape>\r\n    </bpmndi:BPMNPlane>\r\n  </bpmndi:BPMNDiagram>\r\n</bpmn:definitions>";

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
        public async Task<IActionResult> CreateTicketsFromChartTasks(
            int workspaceId, int boardId,
            List<string> selectedTasksIds,
            string xmlChart, IFormFile svgFile)
        {
            if (!ModelState.IsValid)
                return BadRequest("Произошла ошибка");

            var getTitlesResponse = _chartService.GetTicketsFromChart(selectedTasksIds, xmlChart);
            if (getTitlesResponse.StatusCode != Domain.Enums.StatusCode.OK)
                return BadRequest("Произошла ошибка при обработке диаграммы");

            var tickets = getTitlesResponse.Data!.Select(t => t.Name);

            var createTicketsResponse = await CreateTicketsResponseAsync(tickets, boardId);
            if (createTicketsResponse.Count == 0 ||
                !await AddLinksToNewTicketsAsync(workspaceId, createTicketsResponse, getTitlesResponse.Data!))
                return BadRequest("Произошла ошибка при создании карточек или добавлении ссылок");

            var fileResponse = await AddFileToTicketsAsync(svgFile, createTicketsResponse);

            if (!fileResponse)
                return BadRequest("Ошибка при добавлении файла");

            return Ok();
        }


        private async Task<List<int>> CreateTicketsResponseAsync(IEnumerable<string> names, int boardId)
        {
            List<int> ticketIds = new List<int>();

            foreach (var name in names)
            {
                var createResponse = await _boardService.CreateTicket(User.Identity!.Name!, name, boardId);

                if (createResponse.StatusCode != Domain.Enums.StatusCode.OK)
                    return new List<int>(0);

                ticketIds.Add(createResponse.Data!);
            }
            return ticketIds;
        }

        private async Task<bool> AddLinksToNewTicketsAsync(int workspaceId, List<int> ticketsIds, HashSet<TaskNode> ticketsSet)
        {
            var tickets = new List<Ticket>();
            foreach (int id in ticketsIds)
            {
                var ticketResponse = await _boardService.GetTicketById(id);
                if (ticketResponse.StatusCode != Domain.Enums.StatusCode.OK)
                    return false;

                tickets.Add(ticketResponse.Data!);
            }

            // создание ссылок
            foreach (var currentTicket in tickets)
            {
                var linkedTicketNode = ticketsSet.FirstOrDefault(t => t.Name == currentTicket.Title);

                if (linkedTicketNode is null)
                    continue;

                if (linkedTicketNode.Next is not null)
                {
                    var linkedTicketName = linkedTicketNode.Next.Name;

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
                if (linkedTicketNode.Previous is not null)
                {
                    var linkedTicketName = linkedTicketNode.Previous.Name;

                    var linkedTicket = tickets.FirstOrDefault(t => t.Title == linkedTicketName);

                    if (linkedTicket is null)
                        continue;

                    var link = new Link
                    {
                        Name = $"Предыдущий этап (Карточка {linkedTicket!.Id})",
                        Source = $"{HttpContext.Request.Scheme}://{Request.Host}/Workspace/Workspaces/{workspaceId}/{linkedTicket.Id}"
                    };

                    var linkResponse = await _linkService.CreateLink(currentTicket!.Id, link);

                    if (linkResponse.StatusCode != Domain.Enums.StatusCode.OK)
                        return false;
                }
            }

            return true;
        }

        private async Task<bool> AddFileToTicketsAsync(IFormFile formFile, List<int> ticketsIds)
        {
            foreach (int id in ticketsIds)
            {
                var svgFile = new FileAttachment
                {
                    FileName = "Диаграмма процесса",
                    FileContent = new FileContent
                    {
                        Content = GetFileBytes(formFile),
                    }
                };

                var fileResponse = await _fileService.CreateFiles(id, new List<FileAttachment>() { svgFile });

                if (fileResponse.StatusCode != Domain.Enums.StatusCode.OK)
                    return false;
            }

            return true;
        }
        private byte[] GetFileBytes(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
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
