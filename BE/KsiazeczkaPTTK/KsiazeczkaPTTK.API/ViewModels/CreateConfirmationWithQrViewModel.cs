using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class CreateConfirmationWithQrViewModel
    {
        [Required]
        [MaxLength(250)]
        public string Url { get; set; }

        public int TerrainPointId { get; set; }

        public int SegmentId { get; set; }
    }
}
