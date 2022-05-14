using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class TouristsBook
    {
        [Key]
        [MaxLength(30)]
        public string OwnerId { get; set; }

        [ForeignKey("Wlasciciel")]
        public User Owner { get; set; }

        public bool Disability { get; set; }

        public int Points { get; set; }
    }
}
