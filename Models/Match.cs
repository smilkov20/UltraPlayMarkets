using System;
using System.Collections.Generic;
using UltraPlayMarkets.Utilities.Enums;

namespace UltraPlayMarkets.Models
{
    public class Match
    {
        public string Name { get; set; }

        public string OpponentName { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public MatchTypeEnum MatchType { get; set; }

        public int EventId { get; set; }

        public List<Bet> Bets { get; set; }

    }
}
