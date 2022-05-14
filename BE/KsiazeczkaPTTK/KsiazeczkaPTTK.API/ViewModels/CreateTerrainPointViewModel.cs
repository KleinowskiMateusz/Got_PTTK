using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.API.ViewModels
{
    public class CreateTerrainPointViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Range(-90, 90)]
        public double Lat { get; set; }

        [Range(-180, 180)]
        public double Lng { get; set; }
        public double Mnpm { get; set; }
        [MaxLength(30)]
        public string TouristsBookOwner { get; set; }
    }
}
