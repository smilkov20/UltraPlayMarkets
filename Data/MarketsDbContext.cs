using Microsoft.EntityFrameworkCore;
using System.Linq;
using UltraPlayMarkets.Models;
using UltraPlayMarkets.Utilities;

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
        public DbSet<MatchDetails> MatchDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GlobalConstants.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetPreviewMatches>().HasNoKey();
            modelBuilder.Entity<MatchDetails>().HasNoKey();
        }

    }
}
