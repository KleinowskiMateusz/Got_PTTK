using KsiazeczkaPttk.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string TouristsBookId { get; set; }

        [ForeignKey("TouristsBookId")]
        public TouristsBook TouristsBook { get; set; }

        [Required]
        public TripStatus Status { get; set; }

        public IEnumerable<SegmentTravel> Segments { get; set; }
    }
}
