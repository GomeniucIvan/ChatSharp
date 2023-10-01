using System.ComponentModel.DataAnnotations;
using ChatSharp.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSharp.Core.Platform.Messaging.Domain
{
    [Table(nameof(Session))]
    public class Session : BaseEntity
    {
        public Guid Guid { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int AutoDeleteAfterXDays { get; set; }  
        [StringLength(500)]
        public string ModelName { get; set; }
    }
}
