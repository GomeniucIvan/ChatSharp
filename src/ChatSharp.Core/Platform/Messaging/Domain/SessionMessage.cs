using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatSharp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatSharp.Core.Platform.Messaging.Domain
{
    internal class SessionMessageMap : IEntityTypeConfiguration<SessionMessage>
    {
        public void Configure(EntityTypeBuilder<SessionMessage> builder)
        {
            builder.HasOne(c => c.Session)
                .WithMany()
                .HasForeignKey(c => c.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    [Table(nameof(SessionMessage))]
    public class SessionMessage : BaseEntity
    {
        public int SessionId { get; set; }
        public Session Session { get; set; }

        public bool IsMine { get; set; }

        [MaxLength]
        public string Message { get; set; }
    }
}
