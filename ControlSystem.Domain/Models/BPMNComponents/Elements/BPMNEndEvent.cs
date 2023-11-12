namespace ControlSystem.Domain.Models.BPMNComponents.Elements
{
    public class BPMNEndEvent : BPMNElement
    {
        public string Incoming { get; set; }

        public override int Width { get; set; } = 36;
        public override int Height { get; set; } = 36;
    }
}
