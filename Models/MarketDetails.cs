using System;

namespace UltraPlayMarkets.Models
{
    public class MarketDetails
    {
        public bool IsLive { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }

        public int MatchId { get; set; }

        public string PlayerOne { get; set; }

        public string PlayerTwo { get; set; }

        public DateTime StartDate { get; set; }

        public int BetId { get; set; }

        public string BetName { get; set; }

        public int OddId { get; set; }

        public string OddName { get; set; }

        public double Value { get; set; }

        public double SpecialBetValue { get; set; }

        public Odd Odds { get; set; }


    }
}
