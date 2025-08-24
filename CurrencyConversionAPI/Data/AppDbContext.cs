using CurrencyConversionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConversionAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<ConversionRecord> ConversionRecords { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRate>().HasIndex(c => c.CurrencyCode).IsUnique();
            modelBuilder.Entity<ConversionRecord>().HasIndex(c => c.Timestamp);
        }
    }

}
