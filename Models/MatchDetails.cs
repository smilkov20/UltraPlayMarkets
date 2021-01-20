using System;

namespace UltraPlayMarkets.Models
{
    public class MatchDetails
    {
        public bool IsLive { get; set; }

        public int MatchId { get; set; }

        public string PlayerOne { get; set; }

        public string PlayerTwo { get; set; }

        public DateTime StartDate { get; set; }

        public int BetId { get; set; }

        public string BetName { get; set; }

        public string HomecomingName { get; set; }

        public double? HomecomingValue { get; set; }

        public string HomecomingSBV { get; set; }

        public string GuestName { get; set; }

        public double? GuestValue { get; set; }

        public string GuestSBV { get; set; }
    }
}
