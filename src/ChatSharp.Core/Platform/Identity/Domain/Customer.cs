using ChatSharp.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSharp.Core.Platform.Identity.Domain
{
    [Table(nameof(Customer))]
    public class Customer : BaseEntity
    {
        public Guid CustomerGuid { get; set; } = Guid.NewGuid();

        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(500)]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        [StringLength(450)]
        public string PasswordSalt { get; set; }
    }
}
