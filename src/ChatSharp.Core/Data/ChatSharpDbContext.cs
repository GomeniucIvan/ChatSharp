using ChatSharp.Core.Platform.Confirguration.Domain;
using ChatSharp.Core.Platform.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatSharp.Core.Data
{
    public class ChatSharpDbContext: DbContext
    {
        public ChatSharpDbContext(DbContextOptions<ChatSharpDbContext> options) : base(options)
        {
            
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
