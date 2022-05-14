using System.ComponentModel.DataAnnotations;

namespace KsiazeczkaPttk.Domain.Models
{
    public class UserRole
    {
        [Key]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
