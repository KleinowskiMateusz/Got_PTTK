using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class CreateTripViewModel
    {
        public string Wlasciciel { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public IEnumerable<SegmentTravelViewModel> SegmentTravels { get; set; }
    }
}
