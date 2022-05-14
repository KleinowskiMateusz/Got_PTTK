using KsiazeczkaPttk.Domain.Models;
using System.Collections.Generic;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class PotwierdzeniaOdcinkaViewModel
    {
        public SegmentTravel PrzebytyOdcinek { get; set; }
        public IEnumerable<SegmentConfirmation> Potwierdzenia { get; set; }
    }
}
