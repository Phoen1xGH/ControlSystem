namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNProcess : BPMNElement
    {
        public List<BPMNTask>? TaskList { get; set; } = new();
        public BPMNStartEvent StartEvent { get; set; } = new();
        public List<BPMNEndEvent>? EndEventList { get; set; } = new();
        public List<BPMNExclusiveGateway>? ExclusiveGatewayList { get; set; } = new();

        public List<BPMNSequenceFlow>? SequenceFlowList { get; set; } = new();

        public string ProcessType { get; set; } = "None";

        public bool IsClosed { get; set; } = false;

        public bool IsExecutable { get; set; } = false;
    }
}
