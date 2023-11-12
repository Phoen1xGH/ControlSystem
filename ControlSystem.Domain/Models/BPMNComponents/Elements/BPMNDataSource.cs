namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNDataSource : BPMNElement
    {
        public string Incoming { get; set; }
        public string? Outgoing { get; set; }
        public override int Width { get; set; } = 50;
        public override int Height { get; set; } = 50;
    }
}
