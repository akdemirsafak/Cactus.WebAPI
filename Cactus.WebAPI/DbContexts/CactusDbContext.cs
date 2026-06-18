using Cactus.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cactus.WebAPI.DbContexts
{
    public class CactusDbContext : DbContext
    {
        public CactusDbContext(DbContextOptions<CactusDbContext> options) : base(options)
        {
            
        }
        public DbSet<Entities.Event> Events { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "system"; // veya user service
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = "system";
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
