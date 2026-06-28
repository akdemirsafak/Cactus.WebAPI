using Cactus.WebAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Cactus.WebAPI.DbContexts
{
    public class CactusDbContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public CactusDbContext(DbContextOptions<CactusDbContext> options) : base(options)
        {
            
        }
        public DbSet<Entities.Event> Events { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RefreshToken>(
                entity =>
                {
                    entity.Property(x => x.Token)
                        .IsRequired()
                        .HasMaxLength(500);
                    entity.HasIndex(x => x.Token)
                        .IsUnique();
                });
            base.OnModelCreating(builder);
        }

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
