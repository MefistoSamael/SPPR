using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Airplane> airplanes {get; set;}
        public DbSet<EngineTypeCategory> engineTypes {get; set;}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
