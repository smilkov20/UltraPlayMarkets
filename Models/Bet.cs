using System.Collections.Generic;

namespace UltraPlayMarkets.Models
{
    public class Bet
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public bool IsLive { get; set; }

        public List<Odd> Odds { get; set; }

    }
}
