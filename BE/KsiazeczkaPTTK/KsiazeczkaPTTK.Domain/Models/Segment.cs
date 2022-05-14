using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class Segment
    {
        [Key]
        public int Id { get; set; }

        public int Version { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Points { get; set; }

        public int PointsBack { get; set; }

        public bool IsActive { get; set; } = true;

        public int FromId { get; set; }

        [ForeignKey("FromId")]
        public TerrainPoint From { get; set; }

        public int TargetId { get; set; }

        [ForeignKey("TargetId")]
        public TerrainPoint Target { get; set; }

        public int MountainRangeId { get; set; }

        [ForeignKey("MountainRangeId")]
        public MountainRange MountainRange { get; set; }

        [MaxLength(30)]
        public string? TouristsBookOwner { get; set; }

        [ForeignKey("TouristsBookOwner")]
        public TouristsBook? TouristsBook { get; set; }
    }
}
