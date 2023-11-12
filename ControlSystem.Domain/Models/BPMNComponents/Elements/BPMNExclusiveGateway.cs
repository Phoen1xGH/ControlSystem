namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNExclusiveGateway : BPMNElement
    {
        public List<string> Incomings { get; set; }

        public List<string> Outgoings { get; set; }

        public override int Width { get; set; } = 50;
        public override int Height { get; set; } = 50;
    }
}
