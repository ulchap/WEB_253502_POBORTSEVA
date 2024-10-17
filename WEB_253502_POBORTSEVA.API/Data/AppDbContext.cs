using Microsoft.EntityFrameworkCore;
using WEB_253502_POBORTSEVA.Domain.Entities;

namespace WEB_253502_POBORTSEVA.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
    }
}
