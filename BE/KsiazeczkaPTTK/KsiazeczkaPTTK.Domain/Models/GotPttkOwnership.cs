using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class GotPttkOwnership
    {
        [MaxLength(30)]
        public string Owner { get; set; }

        [ForeignKey("Owner")]
        public TouristsBook TouristsBook { get; set; }

        public int GotPttkId { get; set; }

        [ForeignKey("GotPttkId")]
        public GotPttk GotPttk { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? DateOfAward { get; set; }
    }
}
