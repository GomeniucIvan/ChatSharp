using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatSharp.Domain;

namespace ChatSharp.Core.Platform.Confirguration.Domain
{
    [Table(nameof(Setting))]
    public class Setting : BaseEntity
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        [MaxLength]
        public string Value { get; set; }
    }
}
