namespace ControlSystem.Domain.Models.BPMNComponents
{
    public class BPMNElement
    {
        public string Id { get; set; } = "El_" + Guid.NewGuid().ToString();
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public virtual int Width { get; set; } = 0;
        public virtual int Height { get; set; } = 0;

    }
}
