using KsiazeczkaPttk.Domain.Models;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class SegmentConfirmationViewModel
    {
        public SegmentTravel SegmentTravel { get; set; }
        public IEnumerable<SegmentConfirmation> SegmentConfirmations { get; set; }
    }
}
