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

    }
}
