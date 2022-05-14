using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class TerrainPoint
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public double Mnpm { get; set; }

        [MaxLength(30)]
        public string TouristsBookOwner { get; set; }

        [ForeignKey("Wlasciciel")]
        public TouristsBook TouristsBook { get; set; }
    }
}
