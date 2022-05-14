using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.Domain.Models
{
    public class MountainGroup
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
