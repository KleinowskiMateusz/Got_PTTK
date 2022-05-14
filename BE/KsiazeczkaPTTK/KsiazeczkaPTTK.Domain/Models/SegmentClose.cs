using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class SegmentClose
    {
        public int SegmentId { get; set; }

        [ForeignKey("SegmentId")]
        public Segment Segment { get; set; }

        public DateTime ClosedDate { get; set; }

        public DateTime? OpenedDate { get; set; }

        [MaxLength(200)]
        public string Cause { get; set; }
    }
}
