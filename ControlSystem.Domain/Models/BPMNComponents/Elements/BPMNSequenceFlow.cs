namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNSequenceFlow : BPMNElement
    {
        public string SourceRef { get; set; }
        public string TargetRef { get; set; }
    }
}
