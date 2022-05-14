using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class CreateConfirmationWithImageViewModel
    {
        [Required]
        public IFormFile Image { get; set; }

        [MaxLength(250)]
        public string Url { get; set; }

        public int TerrainPointId { get; set; }

        public int SegmentId { get; set; }
    }
}
