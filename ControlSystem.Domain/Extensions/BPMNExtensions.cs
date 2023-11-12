using ControlSystem.Domain.Models.BPMNComponents;
using ControlSystem.Domain.Models.BPMNComponents.Elements;
using System.Xml.Linq;

namespace ControlSystem.Domain.Extensions
{
    public static class BPMNExtensions
    {
        public static void GenerateXmlHeader(this XDocument xml)
        {
            XNamespace bpmn = "http://www.omg.org/spec/BPMN/20100524/MODEL";
            XNamespace bpmndi = "http://www.omg.org/spec/BPMN/20100524/DI";
            XNamespace dc = "http://www.omg.org/spec/DD/20100524/DC";
            XNamespace di = "http://www.omg.org/spec/DD/20100524/DI";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

            XElement definitions = new XElement(
                bpmn + "definitions",
                new XAttribute(XNamespace.Xmlns + "bpmn", bpmn),
                new XAttribute(XNamespace.Xmlns + "bpmndi", bpmndi),
                new XAttribute(XNamespace.Xmlns + "dc", dc),
                new XAttribute(XNamespace.Xmlns + "di", di),
                new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute("targetNamespace", "http://bpmn.io/schema/bpmn")//,
                                                                               //new XAttribute("id", Guid.NewGuid())
            );

            xml.Add(definitions);
        }

        public static void GenerateCollaboration(this XDocument xml, BPMNCollaboration collaboration)
        {
            XElement xmlCollaboration = new XElement(
                XName.Get("collaboration", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                new XAttribute("id", collaboration.Id)
            );

            XElement definitions = xml.Root; // Получаем корневой элемент

            if (definitions != null)
            {
                definitions.Add(xmlCollaboration);
            }
        }

        public static void GenerateParticipants(this XDocument xml, BPMNCollaboration bpmnCollaboration, BPMNProcess process)
        {
            XElement collaboration = xml.Root
                .Elements(XName.Get("collaboration", "http://www.omg.org/spec/BPMN/20100524/MODEL"))
                .First(x => x.Attribute("id")?.Value == bpmnCollaboration.Id);

            foreach (var participant in bpmnCollaboration.Participants)
            {
                participant.ProcessRef = process.Id;
                XElement xmlParticipant = new XElement(
                    XName.Get("participant", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    new XAttribute("id", participant.Id),
                    new XAttribute("name", participant.Name),
                    new XAttribute("processRef", participant.ProcessRef)
                );

                if (collaboration != null)
                {
                    collaboration.Add(xmlParticipant);
                }
            }
        }

        public static void GenerateProcess(this XDocument xml, BPMNProcess bpmnProcess)
        {
            bpmnProcess.Name = $"Process_{Guid.NewGuid()}";

            XElement process = new XElement(
                XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                new XAttribute("id", bpmnProcess.Id),
                new XAttribute("name", bpmnProcess.Name),
                new XAttribute("processType", bpmnProcess.ProcessType),
                new XAttribute("isClosed", bpmnProcess.IsClosed),
                new XAttribute("isExecutable", bpmnProcess.IsExecutable)
            );
            XElement definitions = xml.Root; // Получаем корневой элемент

            if (definitions != null)
            {
                definitions.Add(process);
            }

        }

        public static void GenerateTasks(this XDocument xml, BPMNProcess bpmnProcess)
        {
            // Находим элемент <process> в XML-документе
            XElement process = xml.Root
                .Elements(XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"))
                .First(x => x.Attribute("id").Value == bpmnProcess.Id);

            foreach (var bpmnTask in bpmnProcess.TaskList)
            {
                XElement task = new XElement(
                    XName.Get("task", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    new XAttribute("id", bpmnTask.Id),
                    new XAttribute("name", bpmnTask.Name)
                );

                // Создаем элементы <incoming> и <outgoing>
                XElement incoming = new XElement(XName.Get("incoming", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    bpmnTask.Incoming);
                XElement outgoing = new XElement(XName.Get("outgoing", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    bpmnTask.Outgoing);

                // Добавляем элементы <incoming> и <outgoing> в элемент <task>
                task.Add(incoming, outgoing);

                if (process != null)
                {
                    // Добавляем элемент <task> внутрь элемента <process>
                    process.Add(task);
                }
            }
        }

        public static void GenerateStartEvent(this XDocument xml, BPMNProcess bpmnProcess)
        {
            XElement startEvent = new XElement(
                XName.Get("startEvent", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                new XAttribute("id", bpmnProcess.StartEvent.Id),
                new XAttribute("name", bpmnProcess.StartEvent.Name)
            );

            // Создаем элемент <outgoing>
            XElement outgoing = new XElement(XName.Get("outgoing", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                bpmnProcess.StartEvent.Outgoing);

            // Добавляем элемент <outgoing> в элемент <startEvent>
            startEvent.Add(outgoing);

            // Находим элемент <process> в XML-документе
            XElement process = xml.Root
                .Elements(XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"))
                .First(x => x.Attribute("id")?.Value == bpmnProcess.Id);

            if (process != null)
            {
                // Добавляем элемент <startEvent> внутрь элемента <process>
                process.Add(startEvent);
            }
        }

        public static void GenerateEndEvent(this XDocument xml, BPMNProcess bpmnProcess)
        {
            // Находим элемент <process> в XML-документе
            XElement process = xml.Root
                .Elements(XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"))
                .First(x => x.Attribute("id")?.Value == bpmnProcess.Id);

            foreach (var bpmnEndEvent in bpmnProcess.EndEventList)
            {
                XElement endEvent = new XElement(
                    XName.Get("endEvent", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    new XAttribute("id", bpmnEndEvent.Id),
                    new XAttribute("name", bpmnEndEvent.Name)
                );

                // Создаем элемент <incoming>
                XElement incoming = new XElement(XName.Get("incoming", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    bpmnEndEvent.Incoming);

                // Добавляем элемент <incoming> в элемент <endEvent>
                endEvent.Add(incoming);

                if (process != null)
                {
                    // Добавляем элемент <endEvent> внутрь элемента <process>
                    process.Add(endEvent);
                }
            }
        }

        public static void GenerateExclusiveGateway(this XDocument xml, BPMNProcess bpmnProcess)
        {
            // Находим элемент <process> в XML-документе
            XElement process = xml.Root
                .Elements(XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"))
                .First(x => x.Attribute("id")?.Value == bpmnProcess.Id);

            foreach (var bpmnExclusiveGateway in bpmnProcess.ExclusiveGatewayList)
            {
                XElement exclusiveGateway = new XElement(
                    XName.Get("exclusiveGateway", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    new XAttribute("id", bpmnExclusiveGateway.Id),
                    new XAttribute("name", bpmnExclusiveGateway.Name)
                );

                // Создаем элементы <incoming> на основе массива incomingIds
                foreach (var incomingId in bpmnExclusiveGateway.Incomings)
                {
                    XElement incoming =
                        new XElement(XName.Get("incoming", "http://www.omg.org/spec/BPMN/20100524/MODEL"), incomingId);
                    exclusiveGateway.Add(incoming);
                }

                // Создаем элементы <outgoing> на основе массива outgoingIds
                foreach (var outgoingId in bpmnExclusiveGateway.Outgoings)
                {
                    XElement outgoing =
                        new XElement(XName.Get("outgoing", "http://www.omg.org/spec/BPMN/20100524/MODEL"), outgoingId);
                    exclusiveGateway.Add(outgoing);
                }

                if (process != null)
                {
                    // Добавляем элемент <exclusiveGateway> внутрь элемента <process>
                    process.Add(exclusiveGateway);
                }
            }
        }

        public static void GenerateSequenceFlow(this XDocument xml, BPMNProcess bpmnProcess)
        {
            // Находим элемент <process> в XML-документе
            XElement process = xml.Root
                .Elements(XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"))
                .First(x => x.Attribute("id")?.Value == bpmnProcess.Id);

            foreach (var bpmnSequenceFlow in bpmnProcess.SequenceFlowList)
            {
                XElement sequenceFlow = new XElement(
                    XName.Get("sequenceFlow", "http://www.omg.org/spec/BPMN/20100524/MODEL"),
                    new XAttribute("id", bpmnSequenceFlow.Id),
                    new XAttribute("name", bpmnSequenceFlow.Name),
                    new XAttribute("sourceRef", bpmnSequenceFlow.SourceRef),
                    new XAttribute("targetRef", bpmnSequenceFlow.TargetRef)
                );

                if (process != null)
                {
                    // Добавляем элемент <sequenceFlow> внутрь элемента <process>
                    process.Add(sequenceFlow);
                }
            }
        }

        public static void GenerateBPMNDiagram(this XDocument xml)
        {
            string diagramId = $"Diagram_{DateTime.Now.Ticks}";

            XElement bpmnDiagram = new XElement(
                XName.Get("BPMNDiagram", "http://www.omg.org/spec/BPMN/20100524/DI"),
                new XAttribute("id", diagramId)
            );

            // Находим элемент <definitions> в XML-документе
            XElement definitions = xml.Root;

            if (definitions != null)
            {
                // Добавляем элемент <bpmndi:BPMNDiagram> внутрь элемента <definitions>
                definitions.Add(bpmnDiagram);
            }
        }

        public static void GenerateBPMNPlane(this XDocument xml, BPMNCollaboration collab)
        {
            // Создаем элемент <bpmndi:BPMNPlane> с атрибутами id и bpmnElement
            XElement bpmnPlane = new XElement(
                XName.Get("BPMNPlane", "http://www.omg.org/spec/BPMN/20100524/DI"),
                new XAttribute("id", "El_" + Guid.NewGuid().ToString()),
                new XAttribute("bpmnElement", collab.Id)
            );

            // Находим элемент <bpmndi:BPMNDiagram> в XML-документе
            XElement bpmnDiagram = xml.Root
                .Descendants(XName.Get("BPMNDiagram", "http://www.omg.org/spec/BPMN/20100524/DI"))
                .FirstOrDefault();

            if (bpmnDiagram != null)
            {
                // Добавляем элемент <bpmndi:BPMNPlane> внутрь элемента <bpmndi:BPMNDiagram>
                bpmnDiagram.Add(bpmnPlane);
            }
        }

        public static void GenerateBPMNShape(this XDocument xml, string bpmnElement, bool isHorizontal,
            double x, double y, double width, double height)
        {
            // Находим элемент <bpmndi:BPMNDiagram> в XML-документе
            XElement? bpmnDiagram = xml.Root
                .Descendants(XName.Get("BPMNDiagram", "http://www.omg.org/spec/BPMN/20100524/DI"))
                .FirstOrDefault();

            if (bpmnDiagram != null)
            {
                // Ищем элемент <BPMNPlane> внутри <BPMNDiagram>
                XElement bpmnPlane = bpmnDiagram
                    .Descendants(XName.Get("BPMNPlane", "http://www.omg.org/spec/BPMN/20100524/DI"))
                    .First();

                if (bpmnPlane != null)
                {
                    // Создаем элемент <bpmndi:BPMNShape> с атрибутами id, bpmnElement и isHorizontal
                    XElement bpmnShape = new XElement(
                        XName.Get("BPMNShape", "http://www.omg.org/spec/BPMN/20100524/DI"),
                        new XAttribute("id", "El_" + Guid.NewGuid().ToString()),
                        new XAttribute("bpmnElement", bpmnElement),
                        new XAttribute("isHorizontal", isHorizontal.ToString().ToLowerInvariant())
                    );

                    // Создаем элемент <omgdc:Bounds> с атрибутами x, y, width и height
                    XElement omgdcBounds = new XElement(
                        XName.Get("Bounds", "http://www.omg.org/spec/DD/20100524/DC"),
                        new XAttribute("x", x),
                        new XAttribute("y", y),
                        new XAttribute("width", width),
                        new XAttribute("height", height)
                    );

                    bpmnShape.Add(omgdcBounds);

                    // Добавляем элемент bpmnShape внутрь элемента bpmnPlane
                    bpmnPlane.Add(bpmnShape);
                }
            }
        }


        public static XElement GenerateBPMNLabel(this XDocument xml, string labelStyleId, double x, double y,
            double width, double height)
        {
            // Создаем элемент <bpmndi:BPMNLabel> с атрибутом labelStyle
            XElement bpmnLabel = new XElement(
                XName.Get("BPMNLabel", "http://www.omg.org/spec/BPMN/20100524/DI"),
                new XAttribute("labelStyle", labelStyleId)
            );

            // Создаем элемент <omgdc:Bounds> для bpmnLabel
            XElement omgdcLabelBounds = new XElement(
                XName.Get("Bounds", "http://www.omg.org/spec/DD/20100524/DC"),
                new XAttribute("x", x),
                new XAttribute("y", y),
                new XAttribute("width", width),
                new XAttribute("height", height)
            );

            bpmnLabel.Add(omgdcLabelBounds);

            return bpmnLabel;
        }


        public static void GenerateBPMNEdge(this XDocument xml, string edgeId, string bpmnElement,
            List<Tuple<double, double>> waypoints)
        {
            // Находим элемент <bpmndi:BPMNDiagram> в XML-документе
            XElement bpmnDiagram = xml.Root
                .Descendants(XName.Get("BPMNDiagram", "http://www.omg.org/spec/BPMN/20100524/DI"))
                .FirstOrDefault();

            if (bpmnDiagram != null)
            {
                // Ищем элемент <BPMNPlane> внутри <BPMNDiagram>
                XElement bpmnPlane = bpmnDiagram
                    .Descendants(XName.Get("BPMNPlane", "http://www.omg.org/spec/BPMN/20100524/DI"))
                    .FirstOrDefault();

                if (bpmnPlane != null)
                {
                    // Создаем элемент <bpmndi:BPMNEdge> с атрибутами id и bpmnElement
                    XElement bpmnEdge = new XElement(
                        XName.Get("BPMNEdge", "http://www.omg.org/spec/BPMN/20100524/DI"),
                        new XAttribute("id", edgeId),
                        new XAttribute("bpmnElement", bpmnElement)
                    );

                    // Создаем элементы <omgdi:waypoint> для каждой точки маршрута (waypoint)
                    foreach (var waypoint in waypoints)
                    {
                        XElement omgdiWaypoint = new XElement(
                            XName.Get("waypoint", "http://www.omg.org/spec/DD/20100524/DI"),
                            new XAttribute("x", waypoint.Item1),
                            new XAttribute("y", waypoint.Item2)
                        );

                        bpmnEdge.Add(omgdiWaypoint);
                    }

                    // Добавляем элемент bpmnEdge внутрь элемента bpmnPlane
                    bpmnPlane.Add(bpmnEdge);
                }
            }
        }


        public static void GenerateLabelStyle(this XDocument xml, string labelStyleId,
            string fontName, int fontSize, bool isBold, bool isItalic,
            bool isUnderline, bool isStrikeThrough)
        {

            // Найдем элемент <bpmndi:BPMNDiagram> с указанным id
            XElement bpmnDiagram = xml.Root
                .Descendants(XName.Get("BPMNDiagram", "http://www.omg.org/spec/BPMN/20100524/DI")).FirstOrDefault();

            if (bpmnDiagram != null)
            {
                // Создаем элемент <bpmndi:BPMNLabelStyle> с атрибутом id
                XElement bpmnLabelStyle = new XElement(
                    XName.Get("BPMNLabelStyle", "http://www.omg.org/spec/BPMN/20100524/DI"),
                    new XAttribute("id", labelStyleId)
                );

                // Создаем элемент <omgdc:Font> с атрибутами name, size и стилями шрифта
                XElement omgdcFont = new XElement(
                    XName.Get("Font", "http://www.omg.org/spec/DD/20100524/DC"),
                    new XAttribute("name", fontName),
                    new XAttribute("size", fontSize),
                    new XAttribute("isBold", isBold.ToString().ToLowerInvariant()),
                    new XAttribute("isItalic", isItalic.ToString().ToLowerInvariant()),
                    new XAttribute("isUnderline", isUnderline.ToString().ToLowerInvariant()),
                    new XAttribute("isStrikeThrough", isStrikeThrough.ToString().ToLowerInvariant())
                );

                // Добавляем элемент omgdcFont внутрь bpmnLabelStyle
                bpmnLabelStyle.Add(omgdcFont);

                // Добавляем bpmnLabelStyle в найденный bpmnDiagram
                bpmnDiagram.Add(bpmnLabelStyle);
            }
        }


        public static void GenerateDiagramElements(this XDocument xml, BPMNElementsStorage bpmnElementsStorage)
        {
            xml.GenerateXmlHeader();
            if (bpmnElementsStorage.Collaboration != null)
            {
                xml.GenerateCollaboration(bpmnElementsStorage.Collaboration);
            }

            foreach (var process in bpmnElementsStorage.Processes)
            {
                if (bpmnElementsStorage.Collaboration != null)
                {
                    xml.GenerateParticipants(bpmnElementsStorage.Collaboration, process);
                }
                xml.GenerateProcess(process);
                xml.GenerateTasks(process);
                xml.GenerateExclusiveGateway(process);
                xml.GenerateStartEvent(process);
                xml.GenerateEndEvent(process);
                xml.GenerateSequenceFlow(process);
            }

            xml.GenerateBPMNDiagram();
            xml.GenerateBPMNPlane(bpmnElementsStorage.Collaboration);
        }

        public static void FillSequenceFlows(BPMNProcess process)
        {
            List<BPMNElement> bpmnElements = new List<BPMNElement>();

            bpmnElements.AddRange(process.EndEventList);
            bpmnElements.AddRange(process.ExclusiveGatewayList);
            bpmnElements.AddRange(process.TaskList);
            bpmnElements.Add(process.StartEvent);

            foreach (var sourceElement in bpmnElements)
            {
                foreach (var targetElement in bpmnElements)
                {
                    if (sourceElement is BPMNTask task)
                    {
                        if (task.Outgoing == targetElement.Name)
                            process.SequenceFlowList.Add(GetSequenceFlow(sourceElement, targetElement));
                    }
                    else if (sourceElement is BPMNExclusiveGateway exclusiveGateway)
                    {
                        foreach (var sqf in from outgoing in exclusiveGateway.Outgoings
                                            where outgoing == targetElement.Name
                                            select GetSequenceFlow(sourceElement, targetElement))
                        {
                            process.SequenceFlowList.Add(sqf);
                        }
                    }
                    else if (sourceElement is BPMNStartEvent startEvent)
                    {
                        if (startEvent.Outgoing == targetElement.Name)
                            process.SequenceFlowList.Add(GetSequenceFlow(sourceElement, targetElement));

                    }
                }
            }
        }

        public static void SetDiagramOptions(this XDocument xml, BPMNElementsStorage storage)
        {
            List<BPMNElement> bpmnElements = new List<BPMNElement>();
            foreach (var process in storage.Processes)
            {
                bpmnElements.Add(process.StartEvent);
                bpmnElements.AddRange(process.EndEventList);
                bpmnElements.AddRange(process.ExclusiveGatewayList);
                bpmnElements.AddRange(process.TaskList);

            }

            List<BPMNElement> sortedElements = new List<BPMNElement>();
            //sortedElements.Add(bpmnElements[0]);
            //GetSortedElements(bpmnElements[0], in bpmnElements, sortedElements);
            sortedElements = GetSortedElements(bpmnElements[0], bpmnElements);

            List<string> unsortedNames = bpmnElements.Select(element => element.Name).ToList();
            List<string> sortedNames = sortedElements.Select(x => x.Name).ToList();

            int x = 393;
            int y = 170;

            foreach (var element in sortedElements)
            {
                element.X = x;
                element.Y = y;


                xml.GenerateBPMNShape(element.Id, true, element.X, element.Y - element.Height/2, element.Width, element.Height);

                x += element.Width + 70;
            }

            foreach (var participant in storage.Collaboration.Participants)
            {
                xml.GenerateBPMNShape(participant.Id, true, 330, 70, x, 250);
            }
            xml.SetEdges(storage);
        }

        private static void SetEdges(this XDocument xml, BPMNElementsStorage storage)
        {
            List<BPMNElement> bpmnElements = new List<BPMNElement>();
            foreach (var process in storage.Processes)
            {
                bpmnElements.Add(process.StartEvent);
                bpmnElements.AddRange(process.EndEventList);
                bpmnElements.AddRange(process.ExclusiveGatewayList);
                bpmnElements.AddRange(process.TaskList);

            }

            List<BPMNSequenceFlow> sqfList = new List<BPMNSequenceFlow>();
            foreach (var process in storage.Processes)
            {
                sqfList.AddRange(process.SequenceFlowList);
            }

            foreach (var sqf in sqfList)
            {
                var source = bpmnElements.Where(x => x.Id == sqf.SourceRef).FirstOrDefault();
                var target = bpmnElements.Where(x => x.Id == sqf.TargetRef).FirstOrDefault();
                var wayPoints = new List<Tuple<double, double>>
                {
                    new Tuple<double, double>(source.X + source.Width, source.Y /*+ source.Height/2*/),
                    new Tuple<double, double>(target.X, target.Y /*+ target.Height/2*/)
                };

                if (GetSortedElements(bpmnElements[0], bpmnElements).Last().Id == sqf.TargetRef)
                {
                    var wayPoints2 = new List<Tuple<double, double>>
                    {
                        new Tuple<double, double>(source.X + source.Width/2, source.Y - source.Height/2),
                        new Tuple<double, double>(source.X + source.Width/2 , target.Y - 50 /*+ target.Height/2*/),
                        new Tuple<double, double>(target.X + target.Width/2, target.Y - 50 /*+ target.Height/2*/),
                        new Tuple<double, double>(target.X + target.Width/2, target.Y - target.Height/2)
                    };
                    xml.GenerateBPMNEdge($"El_{Guid.NewGuid()}", sqf.Id, wayPoints2);
                    continue;
                }
                xml.GenerateBPMNEdge($"El_{Guid.NewGuid()}", sqf.Id, wayPoints);
            }



            //var a = new List<Tuple<double, double>>();
            //a.Add(new Tuple<double, double>(x, y));

            //xml.GenerateBPMNEdge($"El_{Guid.NewGuid()}", element.Id, a);
        }



        static List<BPMNElement> GetSortedElements(BPMNElement currentElement, List<BPMNElement> elements)
        {
            List<BPMNElement> sorted = new()
            {
                currentElement
            };

            while (sorted.Count < elements.Count)
            {
                switch (currentElement)
                {
                    case BPMNStartEvent startEvent:
                        {
                            foreach (var element in elements)
                            {
                                if (startEvent.Outgoing == element.Name)
                                {
                                    currentElement = element;
                                    sorted.Add(currentElement);
                                    break;
                                }
                            }
                            break;
                        }
                    case BPMNExclusiveGateway gateway:
                        {
                            foreach (var element in elements)
                            {
                                foreach (var outgoing in gateway.Outgoings)
                                {
                                    if (outgoing == element.Name)
                                    {
                                        currentElement = element;
                                        sorted.Add(currentElement);
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case BPMNTask task:
                        {
                            foreach (var element in elements)
                            {
                                if (task.Outgoing == element.Name)
                                {
                                    currentElement = element;
                                    sorted.Add(currentElement);
                                    break;
                                }
                            }
                            break;
                        }
                }
            }

            return sorted;
        }

        public static BPMNSequenceFlow GetSequenceFlow(BPMNElement sourceElement, BPMNElement targetElement)
        {
            return new BPMNSequenceFlow
            {
                SourceRef = sourceElement.Id,
                TargetRef = targetElement.Id,
                // Name = $"{sourceElement.Name} - {targetElement.Name}"
                Name = ""
            };
        }
    }
}
