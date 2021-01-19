using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public DbSet<GetPreviewMatches> GetPreviewMatches { get; set; }


        private const string ConnectionString = @"Server=DESKTOP-55QSCEM\DEVSERVER;Database=MarketsTask;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetPreviewMatches>().HasNoKey();
        }

    }
}
