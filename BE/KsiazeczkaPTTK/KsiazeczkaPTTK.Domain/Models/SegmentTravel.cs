using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class SegmentTravel
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public int TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        public int SegmentId { get; set; }

        [ForeignKey("SegmentId")]
        public Segment Segment { get; set; }

        public bool IsBack { get; set; }
    }
}
