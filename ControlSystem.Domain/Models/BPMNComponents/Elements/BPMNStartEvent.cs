namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNStartEvent : BPMNElement
    {
        public string Outgoing { get; set; }

        public override int Width { get; set; } = 36;
        public override int Height { get; set; } = 36;
    }
}
