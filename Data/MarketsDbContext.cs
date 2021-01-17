using Microsoft.EntityFrameworkCore;
using UltraPlayMarkets.Models;

namespace UltraPlayMarkets.Data
{
    public class MarketsDbContext : DbContext
    {
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Odd> Odd { get; set; }
        public DbSet<Bet> Bet { get; set; }

        private const string ConnectionString = @"Server=DESKTOP-55QSCEM\DEVSERVER;Database=MarketsTask;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

    }
}
