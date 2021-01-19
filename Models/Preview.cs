using System;

namespace UltraPlayMarkets.Models
{
    public class Preview
    {
        public int MainRow { get; set; }

        public int AllMarkets { get; set; }

        public int HeaderRank { get; set; }

        public int DetailsRank { get; set; }

        public bool IsLive { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }

        public int MatchId { get; set; }

        public string PlayerOne { get; set; }

        public string PlayerTwo { get; set; }

        public DateTime StartDate { get; set; }

        public int BetId { get; set; }

        public string BetName { get; set; }

        public string HomecomingName { get; set; }

        public double? HomecomingValue { get; set; }

        public double? HomecomingSBV { get; set; }

        public string GuestName { get; set; }

        public double? GuestValue { get; set; }

        public double? GuestSBV { get; set; }
    }
}
