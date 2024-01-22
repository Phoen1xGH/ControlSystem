using ControlSystem.Domain.Entities;
using ControlSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpPost]
        public async Task<IActionResult> AddChartToDb(string jsonChart)
        {
            var chart = JsonConvert.DeserializeObject<Chart>(jsonChart);
            if (ModelState.IsValid)
            {
                var currentUserName = User.Identity!.Name!;
                var response = await _accountService.SaveBPMNToDB(currentUserName, chart);

                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    ViewBag.Chart = chart.XmlData;
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
            return View();
        }
    }
}
