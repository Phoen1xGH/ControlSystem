namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNTask : BPMNElement
    {
        public string Incoming { get; set; }
        public string Outgoing { get; set; }

        public override int Width { get; set; } = 100;
        public override int Height { get; set; } = 80;
    }
}
