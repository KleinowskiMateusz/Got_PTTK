using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class User
    {
        [Key]
        [MaxLength(30)]
        public string Login { get; set; }

        [Required]
        [MaxLength(160)]
        public string Password { get; set; }

        [Required]
        [MaxLength(160)]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string UserRoleName { get; set; }

        [ForeignKey("UserRoleName")]
        public UserRole UserRole { get; set; }
    }
}
