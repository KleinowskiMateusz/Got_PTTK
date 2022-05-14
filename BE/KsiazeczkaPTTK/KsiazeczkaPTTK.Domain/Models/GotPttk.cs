using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KsiazeczkaPttk.Domain.Models
{
    public class GotPttk
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string Level { get; set; }
    }
}
