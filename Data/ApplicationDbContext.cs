using Microsoft.EntityFrameworkCore;
using AssetsMonitor.Models;

namespace AssetsMonitor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GlobalQuote> GlobalQuotes { get; set; }
    }
}
