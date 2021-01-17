using Microsoft.EntityFrameworkCore;
using UltraPlayMarkets.Models;

namespace UltraPlayMarkets.Data
{
    public class MarketsDbContext : DbContext
    {
        public MarketsDbContext(DbContextOptions<MarketsDbContext> options)
        : base(options)
        {
        }

        public DbSet<Sport> Sports { get; set; }

        private const string ConnectionString = @"Server=DESKTOP-55QSCEM\DEVSERVER;Database=MarketsTask;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

    }
}
