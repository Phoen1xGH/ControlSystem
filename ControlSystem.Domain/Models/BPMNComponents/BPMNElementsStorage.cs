using ControlSystem.Domain.Models.BPMNComponents.Elements;

namespace ControlSystem.Domain.Models.BPMNComponents
{
    public class BPMNElementsStorage
    {
        public BPMNCollaboration? Collaboration { get; set; }
        public List<BPMNProcess> Processes { get; set; }

    }
}
