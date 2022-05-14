using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class SegmentConfirmation
    {
        public int Id { get; set; }

        [Required]
        public int ConfirmationId { get; set; }

        [ForeignKey("ConfirmationId")]
        public Confirmation Confirmation { get; set; }

        [Required]
        public int SegmentTravelId { get; set; }

        [ForeignKey("SegmentTravelId")]
        public SegmentTravel SegmentTravel { get; set; }
    }
}
