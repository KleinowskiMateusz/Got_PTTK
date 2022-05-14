using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class CreateSegmentViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public int Points { get; set; }
        [Range(0, int.MaxValue)]
        public int PointsBack { get; set; }
        public int FromId { get; set; }
        public int TargetId { get; set; }
        public int MountainRangeId { get; set; }
        [MaxLength(30)]
        public string TouristsBookOwner { get; set; }
    }
}
