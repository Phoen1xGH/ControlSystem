namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNDataObject : BPMNElement
    {
        public string Incoming { get; set; }
        public string? Outgoing { get; set; }
        public override int Width { get; set; } = 36;
        public override int Height { get; set; } = 50;
    }
}
